using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// UstaPlatformm.Services/PricingEngine.cs
using System.Reflection;
using UstaPlatformm.Domain; // Yeni namespace'i kullanır
using System.IO; // Directory, Path ve File için eklendi

namespace UstaPlatformm.Services
{
    // OCP'nin uygulandığı, plug-in mimarisini çalıştıran kritik sınıf
    public class PricingEngine
    {
        private readonly List<IPricingRule> _rules = new();

        public PricingEngine(string pluginPath)
        {
            LoadRules(pluginPath);
        }

        // OCP'nin uygulandığı yer:
        private void LoadRules(string pluginPath)
        {
            if (!Directory.Exists(pluginPath))
            {
                Console.WriteLine($"[PricingEngine] Uyarı: Kural dizini bulunamadı: {pluginPath}");
                return;
            }

            // Klasördeki tüm DLL dosyalarını bul
            var dllFiles = Directory.GetFiles(pluginPath, "*.dll");

            foreach (var file in dllFiles)
            {
                try
                {
                    // DLL'i (Assembly) belleğe yükle
                    var assembly = Assembly.LoadFrom(file);

                    // Bu assembly içindeki IPricingRule arayüzünü uygulayan tipleri bul
                    var ruleTypes = assembly.GetTypes()
                        .Where(t => typeof(IPricingRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in ruleTypes)
                    {
                        // Bulunan tipten bir nesne (instance) oluştur
                        if (Activator.CreateInstance(type) is IPricingRule rule)
                        {
                            _rules.Add(rule);
                            Console.WriteLine($"[PricingEngine] Kural yüklendi: {rule.RuleName} ({Path.GetFileName(file)})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PricingEngine] Hata: Kural yüklenemedi {file}. Hata: {ex.Message}");
                }
            }
        }

        // Kural zincirini (composition) çalıştırır
        public decimal CalculatePrice(IsEmri workOrder)
        {
            decimal finalPrice = workOrder.TemelUcret;
            workOrder.UygulananKurallar.Clear();

            Console.WriteLine($"-- Fiyatlandırma Başladı (Temel: {finalPrice:C}) --");

            // Yüklenen tüm kuralları sırayla uygula
            foreach (var rule in _rules.OrderBy(r => r.RuleName)) // İndirimlerin en son uygulanması için sırala
            {
                decimal priceBefore = finalPrice;
                finalPrice = rule.ApplyRule(workOrder, finalPrice);

                if (priceBefore != finalPrice)
                {
                    var log = $"{rule.RuleName} (-> {finalPrice:C})";
                    workOrder.UygulananKurallar.Add(log);
                    Console.WriteLine($"  -> Kural Uygulandı: {rule.RuleName}. Yeni Fiyat: {finalPrice:C}");
                }
            }

            workOrder.NihaiUcret = finalPrice;
            Console.WriteLine($"-- Fiyatlandırma Bitti (Nihai: {finalPrice:C}) --");
            return finalPrice;
        }
    }
}
