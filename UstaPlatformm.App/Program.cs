using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO; // Path, Directory ve File için ZORUNLU
using UstaPlatformm.Domain;    // Domain varlıklarını kullanmak için
using UstaPlatformm.Services;  // PricingEngine'i kullanmak için

namespace UstaPlatformm.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- UstaPlatformm v1.0 Başlatılıyor ---");
            Console.Title = "UstaPlatformm v1.0";

            // C# 9.0 (init) ve C# 10.0 (new()) özellikleri kullanılıyor
            // Bunlar .NET 8.0'da tam desteklidir.
            var vatandasAhmet = new Vatandas
            {
                Id = 101,
                Ad = "Ahmet",
                Soyad = "Kaya",
                Konum = (15, 20)
            };

            var talep1 = new Talep(vatandasAhmet)
            {
                Aciklama = "Mutfak musluğu sızdırıyor",
                Konum = (15, 20)
            };

            // (Normalde bu Repository'den gelir, SimpleMatchingEngine ile bulunur)
            var ustaAli = new Usta { Id = 1, Ad = "Ali", Cizelgesi = new Cizelge() };

            // --- PLUG-IN DEMO AKIŞI ---

            // 1. Eklenti (Kural) DLL'lerinin yükleneceği yolu belirle.
            // (Çıktı dizini: .../bin/Debug/net8.0/Plugins)
            string executionPath = AppDomain.CurrentDomain.BaseDirectory;
            string pluginPath = Path.Combine(executionPath, "Plugins");
            Directory.CreateDirectory(pluginPath); // Klasör yoksa oluştur

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Kural (Plug-in) Dizini: {pluginPath}");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("SENARYO 1: Sadece 'Haftasonu' kuralı yüklü.");
            Console.WriteLine($"LÜTFEN {pluginPath} dizinine SADECE");
            Console.WriteLine("'UstaPlatformm.Rules.Haftasonu.dll' dosyasını kopyalayın.");
            Console.WriteLine("(Not: Bu DLL'i UstaPlatformm.Rules.Haftasonu/bin/Debug/net8.0/ altinda bulabilirsiniz)");
            Console.WriteLine("Hazır olduğunuzda ENTER'a basın...");
            Console.ResetColor();
            Console.ReadLine();

            // 2. PricingEngine'i başlat. (Sadece Haftasonu kuralını yükleyecek)
            var pricingEngine1 = new PricingEngine(pluginPath);

            // 3. Hafta sonuna bir iş emri oluştur
            var isEmriHaftasonu = new IsEmri(talep1)
            {
                AtananUsta = ustaAli,
                PlanlananZaman = new DateTime(2025, 10, 25, 14, 0, 0) // CUMARTESİ
            };

            // 4. Fiyatı hesapla
            pricingEngine1.CalculatePrice(isEmriHaftasonu); // Temel (100) + Hafta Sonu (75) = 175 TL beklenir

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"NİHAİ FİYAT (Senaryo 1): {isEmriHaftasonu.NihaiUcret:C}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("\nSENARYO 2: 'Sadakat İndirimi' kuralı (DLL) ekleniyor.");
            Console.WriteLine($"ŞİMDİ, ana uygulamayı kapatmadan,");
            Console.WriteLine($"LÜTFEN {pluginPath} dizinine");
            Console.WriteLine("'UstaPlatformm.Rules.Loyalty.dll' dosyasını da kopyalayın.");
            Console.WriteLine("(Not: Bu DLL'i UstaPlatformm.Rules.Loyalty/bin/Debug/net8.0/ altinda bulabilirsiniz)");
            Console.WriteLine("Hazır olduğunuzda ENTER'a basın...");
            Console.ResetColor();
            Console.ReadLine();

            // 5. YENİ BİR PricingEngine başlat. 
            // Bu sefer klasörü taradığında 2 DLL de bulacak.
            Console.WriteLine("\nPricingEngine yeniden başlatılıyor ve kurallar taranıyor...");
            var pricingEngine2 = new PricingEngine(pluginPath);

            // 6. Aynı iş emrini tekrar oluştur (Ahmet sadık müşteri)
            var isEmriHaftasonuIndirimli = new IsEmri(talep1)
            {
                AtananUsta = ustaAli,
                PlanlananZaman = new DateTime(2025, 10, 25, 14, 0, 0) // CUMARTESİ
            };

            // 7. Fiyatı tekrar hesapla
            // (Temel(100) + Hafta Sonu(75)) * Sadakat(%10 indirim) = 157.5 TL beklenir
            // (Not: Kural sırası önemli. Önce zam, sonra indirim.)
            pricingEngine2.CalculatePrice(isEmriHaftasonuIndirimli);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"NİHAİ FİYAT (Senaryo 2): {isEmriHaftasonuIndirimli.NihaiUcret:C}");
            Console.ResetColor();
            Console.WriteLine("\nGördüğünüz gibi, yeni DLL'i bırakmak fiyat hesaplamasını değiştirdi.");
            Console.WriteLine("Ana uygulama (UstaPlatformm.App) yeniden DERLENMEDİ.");
            Console.WriteLine("--- Demo Bitti ---");
            Console.ReadLine();
        }
    }
}
