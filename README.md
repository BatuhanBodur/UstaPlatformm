# UstaPlatformm - Şehrin Uzmanlık Platformu

Bu proje, Nesne Yönelimli Programlama (NYP) ve İleri C# Projesi kapsamında geliştirilmiştir. Arcadia şehrindeki uzmanlar (usta) ile vatandaş taleplerini eşleştiren, dinamik fiyatlama yapabilen ve SOLID prensiplerine uygun, genişletilebilir bir platform simülasyonudur.

## Projenin Temel Amacı ve Mimarisi

Projenin temel amacı, **Açık/Kapalı Prensibini (OCP)** sağlamaktır. Sistem, ana uygulama kodu (`UstaPlatformm.App`) yeniden derlenmeden, yeni iş kurallarının (örneğin yeni fiyatlandırma indirimleri veya zamları) sisteme dahil edilebilmesi üzerine kurulmuştur.

Bu amaçla, aşağıdaki gibi çok katmanlı bir `.sln` çözüm yapısı kullanılmıştır:

* **`UstaPlatformm.Domain`**: Çekirdek varlıkları (`Usta`, `IsEmri` vb.) ve en önemlisi `IPricingRule` gibi arayüzleri (sözleşmeleri) barındırır. Hiçbir projeye bağımlılığı yoktur.
* **`UstaPlatformm.Infrastructure`**: Veri depoları (`Repository`) gibi dışsal bağımlılıkları içerir (Şu an için `InMemory` olarak simüle edilmiştir).
* **`UstaPlatformm.Services`**: `PricingEngine` (Fiyatlandırma Motoru) gibi ana iş mantığı motorlarını içerir.
* **`UstaPlatformm.App`**: Demo akışını çalıştıran ve kullanıcıyla etkileşime giren Konsol uygulamasıdır.
* **`UstaPlatformm.Rules.*`**: (Örn: `Rules.Haftasonu`) Her biri `IPricingRule` arayüzünü uygulayan, bağımsız derlenebilen ve "eklenti" (plug-in) görevi gören sınıf kitaplıklarıdır (DLL).

## Kritik Teknik Gereksinimler ve Uygulamaları

Proje, rehberde belirtilen aşağıdaki İleri C# özelliklerini kullanmaktadır:

* **`init-only` Özelliği (C# 9.0):** `Vatandas.Id`, `Talep.KayitZamani` gibi alanların sadece nesne oluşturulurken atanabilmesi için kullanıldı.
* **`DateOnly` Tipi (C# 10.0 / .NET 6+):** `Cizelge` sınıfında tarih bazlı iş emirlerini yönetmek için kullanıldı.
* **Dizinleyici (Indexer):** `Cizelge` sınıfında, `schedule[DateOnly.Today]` sözdizimi ile o güne ait iş emirlerine kolay erişim sağlamak için uygulandı.
* **Özel `IEnumerable<T>` Koleksiyonu:** `Rota` sınıfı, `new Rota { (10, 20), (30, 40) }` gibi koleksiyon başlatıcıları desteklemek için `IEnumerable` arayüzünü ve `Add` metodunu uyguladı.
* **Statik Yardımcılar:** `Guard` sınıfı, null kontrolleri için statik bir yardımcı olarak tasarlandı.

## Kurulum ve Çalıştırma (Demo Akışı)

Proje, `.NET 8.0` (veya 6.0+) SDK'sı ve Visual Studio 2022 gerektirir.

1.  Projeyi klonlayın ve `UstaPlatformm.sln` dosyasını Visual Studio ile açın.
2.  Tüm projelerin derlenmesi için üst menüden `Build` -> `Rebuild Solution` (Çözümü Yeniden Derle) deyin.
3.  `UstaPlatformm.App` projesini "Başlangıç Projesi" (Startup Project) olarak ayarlayın ve çalıştırın (F5).
4.  Konsol ekranı açılacak ve **SENARYO 1** için sizden `ENTER` tuşuna basmanızı bekleyecektir.

### Demo: Plug-in Mimarisi Testi

Bu, projenin ana test senaryosudur:

1.  **SENARYO 1 (Tek Kural Yükleme):**
    * Program `ENTER` beklerken, Dosya Gezgini'ni açın.
    * **KAYNAK:** `UstaPlatformm.Rules.Haftasonu\bin\Debug\net8.0\` klasöründeki `UstaPlatformm.Rules.Haftasonu.dll` dosyasını **kopyalayın**.
    * **HEDEF:** `UstaPlatformm.App\bin\Debug\net8.0\Plugins\` klasörüne **yapıştırın**.
    * Konsola dönüp `ENTER`'a basın.
    * **Çıktı:** Programın DLL'i bulduğunu (`[PricingEngine] Kural yüklendi...`) ve fiyatı **175,00 TL** olarak hesapladığını göreceksiniz.

2.  **SENARYO 2 (İkinci Kuralı Yükleme):**
    * Program **SENARYO 2** için tekrar `ENTER` bekleyecek.
    * **KAYNAK:** `UstaPlatformm.Rules.Loyalty\bin\Debug\net8.0\` klasöründeki `UstaPlatformm.Rules.Loyalty.dll` dosyasını **kopyalayın**.
    * **HEDEF:** Aynı `Plugins` klasörüne (ilk DLL'in yanına) **yapıştırın**.
    * Konsola dönüp `ENTER`'a basın.
    * **Çıktı:** Programın yeniden başladığını, bu kez **iki DLL'i de** bulduğunu ve fiyatı (Hafta sonu zammı + Sadakat indirimi) **157,50 TL** olarak hesapladığını göreceksiniz.

## Tasarım Kararları: Dinamik Fiyatlandırma Motoru (OCP)

Projenin en kritik teknik başarımı, `PricingEngine` sınıfıdır. Bu motor, Açık/Kapalı Prensibini (OCP) sağlamak için **Reflection (Yansıma)** kullanır:

1.  **Sözleşme:** `UstaPlatformm.Domain` projesi, tüm kuralların uyması gereken bir `IPricingRule` sözleşmesi (arayüzü) tanımlar.
2.  **Keşif:** `PricingEngine`, uygulama başladığında `Plugins` klasörünü tarar (`Directory.GetFiles("*.dll")`).
3.  **Yükleme:** `Assembly.LoadFrom()` ile bulduğu her DLL'i belleğe yükler.
4.  **Aktivasyon:** Yüklediği DLL'lerin içinde `IPricingRule` arayüzünü uygulayan sınıfları bulur (`Activator.CreateInstance()`) ve bunları bir listeye ekler.
5.  **Yürütme:** Fiyat hesaplaması istendiğinde, bu listedeki tüm kuralları (kural zinciri) sırayla uygular.

Bu tasarım sayesinde, gelecekte "Bayram Zammı" veya "Akşam Mesaisi" gibi yeni kurallar eklemek için, ana uygulamayı durdurmaya veya yeniden derlemeye gerek kalmadan, sadece yeni bir DLL dosyasını `Plugins` klasörüne bırakmak yeterli olacaktır.

##Demo Çıktısı
<img width="956" height="794" alt="Ekran görüntüsü 2025-10-24 165538" src="https://github.com/user-attachments/assets/4b7368ee-c87a-4e09-9643-bed84a4a3390" />

