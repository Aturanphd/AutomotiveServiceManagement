# Claude için proje tanıtımı ve bağımsız inceleme kapsamı

Öğrenci: Ahmet Turan. Proje: Automotive Service Management System / Servis Atölyesi.

Nihai teslim kararı: hoca projeyi bilgisayarında yerel çalıştırarak inceleyecek. Hosting/domain/canlı yayın kapsam dışında; responsive arayüz korunuyor. Kurulum için docs/BILGISAYARDA-CALISTIRMA.md dosyasını da incele.

İnceleme sonrası güncelleme: önceki Claude raporu kullanıcı tarafından iletildi, kodla karşılaştırıldı ve düzeltmeler uygulandı. docs/CLAUDE-REVIEW-DECISIONS.md dosyasını oku. Güncel doğrulama 31 entegrasyon senaryosu + 40 HTTP kontrolüdür; aşağıdaki eski 30/38 sayıları önceki sürümün tarihsel sonuçlarıdır. AddServicePartForm yalnızca servis parça seçimini doğrular; QuantityForm stok girişi için PartId gerektirmez.

## Önce okunacak dosyalar
1. Bu belge: amaç, sınırlar ve inceleme beklentisi.
2. docs/FILE-GUIDE.md: paketteki her dosyanın görevi.
3. docs/MASTER-SPECIFICATION-v1.md ve docs/ER-MODEL.md: kapsam ve veritabanı tasarımı.
4. README.md: kurulum, çalıştırma ve test komutları.
5. src altındaki gerçek kaynaklar ve tests/IntegrationTests/Scenarios.cs.
6. docs/VALIDATION.md: önceki kontrollerin kaydı; bağımsız incelemenin yerine geçmez.

## Amaç ve resmi gereksinimler
MCSD bitirme projesi; database, OOP, HTML/CSS ve sıkıştırılmış proje yanında ayrıca ekran görüntüsü gerektirir. Resmi yönergede konu seçimi serbesttir; web örnekleri ve WinForms CRM/stok örnekleri vardır. Bu uygulama web tabanlı otomotiv servis CRM/stok yönetimidir. Kategori kabulü ve notu eğitmen belirler. Kaynak MCSD.pdf bu pakette bulunmaz; resmi metinle karşılaştırmak istersen kullanıcıdan PDF'yi ayrıca iste. Belgedeki özeti orijinal PDF'yi görmüş gibi sunma.

## Kullanıcı ve iş akışı
Yerel bilgisayarda servis danışmanı: müşteri oluştur → araç ekle → servis aç → kullanılan parçaları ekle → tamamla veya iptal et. Parçalar ayrı ekrandan tanımlanır; yeni parça sıfır stokla açılır, stok girişi detaydan yapılır. Listeler 10 kayıtla sayfalanır. Arayüz ve hata/yönlendirme metinleri Türkçedir.

## Teknoloji ve katmanlar
.NET 10, C#, ASP.NET Core MVC, EF Core 10.0.13, SQLite, Razor, yerel Bootstrap ve CSS. SQL Server kurulumu gerekmez; SQLite ilişkisel SQL veritabanıdır.

```text
Web.UI controller -> Core servis interface'i -> Business service -> Data DbContext -> SQLite
```

Core: entity, form modelleri ve arayüzler. Data: şema, migration, örnek veri. Business: kurallar ve sorgular. Web.UI: form akışı ve görünüm. Core arayüzleri IServices.cs içinde topludur; beş ayrı interface vardır. Çok sayıda class/interface aynı dosyada bulunabilir; bunu otomatik işlevsel hata olarak sınıflandırma.

Gereksiz CQRS, MediatR, microservice veya genel repository ekleme. Eğitim seviyesine uygun, öğrencinin açıklayabileceği sade yapı korunmalı.

## Kritik kurallar — gerçekten uygulandığını kontrol et
- Plaka normalize ve benzersiz; geçersiz müşteri ve azalan kilometre reddedilir.
- Bağlı aracı olan müşteri, servis geçmişi olan araç, stoklu/kullanılmış parça silinmez.
- Bir araçta bir açık servis; gelecek tarih ve eski kilometreyle yeni kayıt açılamaz.
- Servis toplamı sunucuda: işçilik + miktar × satırın tarihi fiyatı. Para kuruş cinsinden int, toplam long.
- Parça ekleme stok düşer; yetersiz stokta kısmi kayıt olmaz. Aynı parça bir dosyaya bir kez eklenebilir.
- İptal parçaları bir kez iade eder; tamamlanan/iptal dosya değişmez. Hatalı açık dosya iptal edilip yeniden açılır; satır düzenleme/çıkarma v1'de yoktur.
- Guid Version ile eşzamanlılık, FK/benzersiz indeks/CHECK kısıtları ve transaction kullanılır. Gerçek yarış durumlarında davranışını ayrıca değerlendir.
- Stok <= eşik kritik; stok sıfırsa tükendi. Liste uyarısı açık sayfayı, dashboard en fazla 10 kritik parçayı gösterir.
- Ek demo katalog kodları mevcut fiyat/stoklara dokunmadan tamamlanır. Bu yaklaşımın yeniden başlatma, silinen demo parça, kısmi veritabanı ve eşzamanlı başlangıç durumlarını özellikle incele.
- Tüm değişiklik POST, global anti-forgery doğrulaması ve ayrı form modelleri ile yapılır. GET veri değiştirmemelidir.
- Para binder'ı virgül/nokta ile en fazla iki ondalık kabul eder; binlik ayırıcı ve belirsiz tutar reddedilir.

## Bilinen sınırlar
Kimlik doğrulama/rol, internet dağıtımı, muhasebe/tahsilat/vergi/fatura, araç-parça uyumluluğu v1 kapsamında değildir. Yerel eğitim demosu olduğu açıktır. İnternete yayın güvenliğini ayrı kapsam olarak belirt; mevcut kapsamda oturum açma yokluğunu gizleme. Profildeki AT bir kimlik doğrulama göstergesi değildir. Örnek fiyatlar kurgusaldır.

## API anahtarı ve dış servisler
Uygulama OpenAI/Claude veya başka bir harici servis API'si kullanmaz; API anahtarı gerektirmez. Varsayılan bağlantı yerel SQLite dosyasıdır; sunucu kullanıcı adı/parolası yoktur. İlk restore NuGet erişimi gerektirir; bu, uygulama içi AI API kullanımı değildir. Program.cs dış servis bağlantı ayarı yerine yerel bağlantı yolu ve isteğe bağlı ConnectionStrings__ServiceDb ortam değişkenini okur.

## Çalıştırma ve test
```powershell
dotnet restore AutomotiveServiceManagement.sln
dotnet build AutomotiveServiceManagement.sln
dotnet run --project tests/IntegrationTests --no-build
python tools/http_smoke.py
dotnet run --project src/Web.UI --no-launch-profile --urls http://127.0.0.1:5180
```
Entegrasyon testi xUnit değildir: console senaryolarıdır, hata durumunda program başarısız çıkar. HTTP testi Python 3 standart kütüphanesiyle 5181 portunda ayrı süreç/geçici SQLite kullanır. En son çalıştırılan kontroller: 30 entegrasyon senaryosu (stok güncellemesinde), 38 HTTP kontrolü ve sıfır hata/uyarılı build (arayüz güncellemesinde). Son otomobil değişiklikleri yalnızca SVG'dir. Bu sonuçları kendi ortamında çalıştırmadıysan kendin doğrulamış gibi yazma.

## İstenen inceleme çıktısı
Önce dosya erişimini doğrula: ZIP'i açabiliyor musun, Core/Data/Business/Web.UI ve testleri gerçekten okuyabiliyor musun? Erişemiyorsan bunu açıkça söyle; kod uydurarak değerlendirme yapma. ZIP okunamazsa ayrıca sağlanan Ahmet_Turan_Claude_Source.txt metin dökümünü kullan.

Kod değiştirmeden bağımsız bir inceleme yap. Bulguları P1 (veri kaybı/ciddi hata), P2 (işlevsel sorun), P3 (küçük sorun) şeklinde sırala. Her bulgu için dosya + doğrulanmış satır, tetikleyen senaryo, beklenen/mevcut davranış ve sade çözüm önerisi ver. İşlevsel hataları, eğitim/sunum önerilerini ve kapsam dışı üretim önerilerini ayır. Özellikle stok işlemleri, eşzamanlılık, seed, para ve Türkçe model binding, doğrulama, CSRF, overposting, hata yönetimi ve test boşluklarına bak. Mevcut CSS üstüne premium.css uygulanır; stil sırasını dikkate al. Görsel değerlendirmeyi screenshots/ ile gerçek dosyaları birlikte okuyarak yap.

Sonuçta: (1) öncelikli bulgular, (2) test edilen/edilemeyenler, (3) resmi gereksinim uyumu, (4) sade iyileştirmeler, (5) öğrenciye sorulabilecek 10 savunma sorusu ver. Not garantisi verme. Uygulamayı veya veritabanını izinsiz sıfırlama; yayımlama/mesaj gönderme yapma.
