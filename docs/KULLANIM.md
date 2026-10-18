# Kullanım kılavuzu — Ahmet Turan

## Örnek demo
İlk açılışta Deniz Yılmaz ve Ece Kaya müşterileri, Deniz'in Renault Clio aracı, motor yağı/yağ filtresi parçaları, bir açık ve bir tamamlanan servis gelir. Bilgiler kurgusaldır. Tamamlanan örnek serviste 800 TL işçilik + 4 × 450 TL yağ = 2.600 TL tutar bulunur. Yağ stok 16, filtre stok 3 adettir; filtre 5 eşik altında olduğu için uyarılır.

## Müşteri ve araç
Ad soyad ve telefon zorunludur. E-posta isteğe bağlıdır. Müşteri detayında araçları, araç detayında servis geçmişini görürsünüz. Liste ekranlarında arama ve 10 kayıtlık sayfalama vardır. Plaka boşluk/tirelerden arındırılarak büyük harfle saklanır. Müşteri veya araç seçilmeden bağlı kayıt açılamaz.

Örnek parça kataloğu 14 çeşit içerir: motor yağı, dört filtre, ön/arka balata, buji, akü, antifriz, fren hidroliği, silecek, ampul ve kayış. Bunlar kurgusal stok/fiyat örnekleridir; araç uyumluluk kataloğu değildir. Eksik ek demo kodları başlangıçta tamamlanır; mevcut parça stokları ve fiyatları sıfırlanmaz. 10 kayıtlık sayfalama nedeniyle diğer parçalar Sonraki bağlantısındadır.

Parça listesinde stok kritik eşiğe eşit veya daha düşük olduğunda sarı uyarı ve satır vurgusu görünür. Durum sütunu Kritik stok, Stok tükendi veya Yeterli stok bilgisini metinle gösterir. Stok sıfırsa kırmızı etiket görünür. Üst uyarı yalnızca açık sayfadaki kayıtları sayar; dashboard kritik parçaları ayrıca listeler.

## Servis
Araç detayındaki Servis aç düğmesi mevcut kilometreyi forma taşır. Kilometreyi gerçek kabul değerine yükseltin; geçmiş değerden düşük girilemez. Tarih en erken 2000, en geç bugündür. Araçta açık dosya varsa ikinci açık dosya oluşturulmaz. İşçilik TL, örneğin `1200,50` olarak girilir. Para en fazla iki ondalık basamak içerir.

Parçanın miktarını eklemeden kontrol edin. Eklenince stok azalır ve satırın fiyatı sabitlenir. Sonraki fiyat değişikliği eski servis toplamını değiştirmez. Aynı parçanın ikinci eklenmesi engellenir. Hatalı miktar için açık dosyayı iptal edip yeni dosya açın. İptalde stok bir kez iade edilir; kilometre otomatik geri alınmaz. Tamamla ve İptal işlemleri onay sorar ve dosyayı kilitler. Geçmişi korumak için servis silme yoktur.

## Parça ve stok
Önce kod/ad/fiyat/eşik ile parça oluşturun. Başlangıç stoğu sıfırdır. Detay ekranında gelen adedi girerek stoğa ekleyin. Çıkış yalnızca servis üzerinden olur. Negatif veya sıfır giriş miktarı kabul edilmez. Tek işlemde 10.000, toplam stokta 1.000.000 adet sınırı vardır. Kullanılmış veya stoklu parça silinemez. Dashboard en fazla 10 kritik parçayı gösterir; diğer parçalar listeden incelenebilir.

## Hata giderme
- Kayıt değişti: sayfayı yenileyin; başka sekmede kaydedilen son bilgiyi kontrol edin.
- Yetersiz stok: parça detayından teslim alınan miktarı girin.
- Kayıt silinemez: bağlı araç/servis geçmişi korunur.
- 5180 portu dolu: uygulamayı farklı yerel portla başlatın (`--urls http://127.0.0.1:5182`).
- Paket indirilemiyor: ilk kurulum için internet/NuGet erişimini kontrol edin.
- Form süresi/token hatası: formu yeniden açın. Geri tuşuyla eski formu tekrar göndermeyin.

## Yedek ve teslim
Yedek almadan uygulamayı kapatın. App_Data klasörünü güvenli bir yere kopyalayın. Kaynak ZIP'ine gerçek müşteri veya veritabanı bilgisi eklemeyin. Teslimde Ahmet_Turan_AutomotiveServiceManagement.zip ve Ahmet_Turan_Screenshot.jpg dosyalarını birlikte kullanın.
