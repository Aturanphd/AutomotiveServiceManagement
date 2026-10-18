# v1 doğrulama raporu

## Son teslim doğrulaması

Claude raporu sonrası güncel sürüm: 31 SQLite entegrasyon senaryosu, 40 HTTP kontrolü ve sıfır hata/uyarılı build başarılı. Kararlar docs/CLAUDE-REVIEW-DECISIONS.md içinde. Aşağıdaki 30/38 sayıları önceki teslim doğrulamasıdır.
Nihai kapsam: bilgisayarda yerel çalıştırma; hosting/canlı yayın kapsam dışı. Güncel kaynakta solution build sıfır hata/uyarı, 30 SQLite entegrasyon senaryosu ve 38 HTTP kontrolü başarılı. Bu son çalıştırma responsive CSS ve otomobil çizimi dahil mevcut sürüme aittir. Teslim ve Claude inceleme ZIP'leri aynı kaynaklardan yeniden üretilir. Kurulum: docs/BILGISAYARDA-CALISTIRMA.md.

Tarih: 16 Ekim 2026. Öğrenci: Ahmet Turan. Ortam: Windows x64, .NET SDK 10.0.401, net10.0, EF Core 10.0.13, SQLite. Testler gerçek ilişkisel SQLite üzerinde çalıştırıldı; EF InMemory kullanılmadı.

## Derleme ve veri
- İlk boş solution derlendi.
- Core/Data/Business ve MVC/Razor ekranları derlendi; şablondan kalan hata görünümü temizlendi.
- InitialCreate migration üretildi ve boş veritabanına uygulandı.
- Son derleme: sıfır uyarı, sıfır hata.
- Paket denetiminde ilk sürümlerde görülen SQLite/Pkcs uyarıları güncel bağımlılıklarla giderildi; uyarı bastırma eklenmedi.

## 29 entegrasyon senaryosu
Migration ve tekrarlanabilir seed; müşteri oluşturma/arama/düzenleme/silme, telefon/e-posta doğrulaması ve eski sürüm reddi; plaka normalleştirme/benzersizlik, müşteri FK, yıl ve kilometre; bağlı müşteri/araç silme koruması; parça oluşturma/fiyat/benzersiz kod; negatif ve eski sürümlü stok girişi; servis tarihi/kilometresi/işçilik; aynı araçta ikinci açık servis; atomik stok düşme, yetersiz stokta değişmeme; servis toplamı, tarihi fiyatı koruma, tekrar parça eklemeyi engelleme; iptalde bir kez iade; kapalı kaydı kilitleme; kullanılmış parça silmeme; dashboard toplamı; geçersiz durum; sayfa sınırları; iki DbContext arasında kayıp güncelleme koruması; SQL CHECK ve FK kısıtları.

Çalıştırma: `dotnet run --project tests/IntegrationTests --no-build`. Konsol başarısızlıkta hata koduyla çıkar; bu bir xUnit projesi değildir.

## 35 HTTP kontrolü
10 ekran/form GET isteği; kayıp kayıtta 404; silme GET isteğinde 405; 10 değişiklik uç noktasında tokensız POST için 400; Türkçe sunucu doğrulaması; müşteri ve araç oluşturma; belirsiz binlik/ondalık fiyat reddi; virgüllü TL girişi; overposting ile gönderilen stoğun yok sayılması; stok girişi; servis açma/parça ekleme/doğru toplam/stok düşümü/tamamlama; dashboard tutarı; bağlı müşteri silme koruması.

Çalıştırma: `python tools/http_smoke.py`. Her çalıştırma geçici SQLite dosyası ve 5181 portunda ayrı süreç kullanır; sonunda süreci kapatır. Demo veritabanı değişmez.

## Tarayıcı ve görsel kontrol
Dashboard, müşteri/araç/servis/parça listeleri ve servis detay ekranı çalışan uygulamada açıldı. Boş parça formu gönderildi; Türkçe doğrulama mesajları görüldü. Ekranlar screenshots/ altında kaydedildi. Mobil dashboard 390×844 görünümde kontrol edildi; sayfa genişliği 390 px, yatay taşma yok. Tablo ekranları küçük ekranlarda kendi alanında yatay kaydırılır. Görsel denetim tam bir erişilebilirlik veya tüm cihazlar için sertifikasyon değildir.

## Sınırlar

### Stok listesi iyileştirmesi
Listeye görünür kritik stok uyarısı, sarı satır vurgusu ve metinli durum etiketleri eklendi. Örnek katalog 14 parçaya genişletildi. Mevcut stok/fiyatın tekrar seed sırasında korunması, sıfır stok, yeterli stok ve eşiğe eşit stokta uyarı senaryoları doğrulandı. Güncel sonuç: **30 entegrasyon senaryosu + 38 HTTP kontrolü geçti; build sıfır hata/uyarı.** Tarayıcıda mevcut yağ filtresinin 2 adet/eşik 5 ile işaretlendiği ve kullanıcının 5.430 TL servis toplamının korunduğu görüldü. stock.jpg ve teslim kaynak ZIP'i güncellendi.
Kimlik doğrulama, internet dağıtımı, faturalama/vergilendirme/tahsilat kapsam dışında. Testler bütün olası hataların yokluğunu veya puanı garanti etmez. Kullanıcı bağımsız Claude kaynak incelemesini iletti; Claude uygulamayı çalıştırmadı. Resmi kategori kabulünü eğitmen değerlendirir. Kaynak ZIP'i gerçek veritabanı yedeği değildir.

## Teslim paketi kontrolü

### Telefon ve tablet iyileştirmesi
Telefon/tablet/bilgisayar boyutlarında 73 sayfa/viewport kontrolünde sayfa dışına taşma görülmedi. 1024 px ve altında menü/düğme dokunma alanları en az 44 px; tablet üst menüye geçer ve tablolar yana kaydırma yönlendirmesi gösterir. Mobil parça formunda gerçek hatalı POST ile Türkçe mesajlar doğrulandı. İş mantığı veya DB değiştirilmedi. Ayrıntılar: docs/CIHAZ-UYUMLULUGU.md. Gerçek fiziksel cihaz, iOS Safari ve uzaktan yayın erişimi henüz doğrulanmadı.

### Arayüz yenilemesi
Petrol yeşili/bakır palet, özgün yerel SVG otomobil çizimi, dekoratif ikonlar, daha okunaklı tablo/form yazıları ve yeni dashboard karşılama alanı uygulandı. İnternetten font/görsel/CDN yüklemesi eklenmedi. Hover efektleri hareket azaltma tercihine uyar. Yeni görünümde build sıfır hata/uyarı ve 38 HTTP kontrolü başarılı. Masaüstü 1440 px ve mobil 390 px dashboard incelendi; yatay sayfa taşması yok. Liste ekranları, servis detayı ve hatalı parça formu tarayıcıda kontrol edildi; ekran görüntüleri yenilendi. İş kuralları ve veritabanı şeması değiştirilmedi.
Ahmet_Turan_AutomotiveServiceManagement.zip ayrı bir klasöre açıldı; restore/build sıfır hata ve uyarıyla, 29 entegrasyon senaryosu da başarıyla tamamlandı. ZIP CRC kontrolü geçti. bin/obj, App_Data, veritabanı ve önbelleklerin arşivde olmadığı doğrulandı. Ayrı Ahmet_Turan_Screenshot.jpg görüntüsü görsel olarak incelendi. Kaynaklarda parola/API anahtarı/gerçek sunucu bağlantısı desen taramasında eşleşme bulunmadı; bu tarama kapsamlı güvenlik denetimi değildir.
