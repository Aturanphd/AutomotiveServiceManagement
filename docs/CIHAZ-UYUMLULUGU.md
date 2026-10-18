# Telefon, tablet ve bilgisayar kontrolü

16 Ekim 2026. Test yöntemi: çalışan ASP.NET Core uygulaması üzerinde tarayıcı viewport boyutları; gerçek fiziksel cihaz testi değildir.

## Yapılan kontroller
360, 390, 600, 768, 820, 1024 ve 1440 px genişliklerde dashboard, dört liste, araç formu ve servis açılış formu: 49 kontrol. 360, 768 ve 1024 px genişliklerde müşteri/parça formları, müşteri/araç/parça detayları, açık/kapalı servis dosyaları ve rehber: 24 kontrol. Toplam 73 sayfa/boyut kontrolünde sayfa genelinde yatay taşma yok. Geniş tablolar kendi alanlarında yatay kaydırılır; bütün sütunların aynı anda görünmesi beklenmez.

1024 px ve altındaki ana menü bağlantıları ve düğmelerin en az 44 px yüksekliği ölçüldü. Tablet dikey/yatay genişliklerinde üst menü tam genişlikte içerik sağlar. Telefon form alanları 16 px yazıyla görüntülenir; bu, mobil tarayıcılarda küçük yazı kaynaklı odak yakınlaştırmasını azaltmayı amaçlar. iOS Safari üzerinde fiilen test edilmedi.

390 px telefon görünümünde boş parça formu gönderildi; Türkçe sunucu doğrulaması ve geri dönen form kontrol edildi. Bu işlem kayıt eklemedi. İş kurallarının önceki HTTP/SQLite testlerine ek olarak ekran yerleşimi kontrolüdür; 73 uçtan uca iş senaryosu anlamına gelmez.

Görseller: screenshots/tablet-dashboard.jpg, screenshots/mobile-dashboard.jpg ve screenshots/mobile-form-validation.jpg.

## Hocanın kendi cihazından açması
Responsive tasarım cihazın ekranına uyum sağlar; siteye ağ erişimi ayrı bir koşuldur. `127.0.0.1` her cihazın kendisini ifade eder. Öğrencinin bilgisayarında çalışan `127.0.0.1:5180` adresi hocanın telefonunda aynı uygulamayı açmaz. ZIP ve GitHub kaynak kodunu sağlar; çalışan web sitesi sunmaz. Telefon/tablete .NET SDK kurup solution açılması beklenmez.

### Aynı yerel ağda gösterim
Bilgisayar ve telefon/tablet aynı güvenilir Wi-Fi ağında olmalı. Sunucuya yerel ağ arayüzünden erişim ayarlanmalı, bağlantıda bilgisayarın LAN IP'si kullanılmalı ve gerekiyorsa yalnızca ilgili port için yerel ağ güvenlik duvarı izni verilmelidir. İzole/misafir ağları cihazlar arası erişimi engelleyebilir. Bu çalışma sırasında ağ dinleme adresi değiştirilmedi ve güvenlik duvarı açılmadı.

### Hocanın uzaktan kontrolü
Sunucuda çalışan ASP.NET Core uygulaması, kalıcı veritabanı/depolama, HTTPS ve erişim yetkilendirmesi gerekir. Mevcut sürüm kimlik doğrulamasız yerel eğitim demosudur. Uzaktan değerlendirme için önce bu dağıtım hazırlığı tamamlanıp gerçek bağlantı cihazlardan test edilmelidir. Şu anda yayımlanmış bir internet adresi yoktur.

## Son cihaz kabul kontrolü
Yayın hazır olduğunda gerçek Android Chrome ve iPhone/iPad Safari ile menü, dokunma, seçim kutusu, tarih/para girişi, klavye açılması, tablo kaydırma, form hata mesajları ve servis işlemleri test edilmelidir. Kamera/uygulama kurulumu gerekmez; tarayıcı yeterlidir. Fiziksel cihaz test sonuçları gerçekleşmeden bu rapora eklenmez.
