# Ahmet Turan — bilgisayarda çalıştırma

Bu teslim ASP.NET Core MVC kaynak projesidir. Hosting hesabı, domain, API anahtarı veya SQL Server kurulumu gerekmez. Uygulama bilgisayarda çalışır; aynı bilgisayarın tarayıcısından kullanılır.

## 1. ZIP'in tamamını çıkartın
Ahmet_Turan_AutomotiveServiceManagement.zip dosyasını sağ tıklayıp Tümünü Ayıkla/Extract All seçin. ZIP'in içinden yalnızca solution dosyasını açmayın veya ayrı yere taşımayın.

Çıkartılan AutomotiveServiceManagement klasöründe şu yapı birlikte bulunmalıdır:
```text
AutomotiveServiceManagement.sln
README.md
Directory.Build.props
global.json
src/
  Core/Core.csproj
  Data/Data.csproj
  Business/Business.csproj
  Web.UI/Web.UI.csproj
tests/
  IntegrationTests/IntegrationTests.csproj
docs/
screenshots/
```

## 2. Gereken ortam
.NET 10 SDK kurulu olmalıdır; yalnızca runtime yeterli değildir. `dotnet --list-sdks` ile kontrol edilebilir. İlk NuGet paket indirme için internet gerekir. Python, siteyi açmak için gerekmez; yalnızca isteğe bağlı HTTP test aracı içindir.

## 3. Terminalden başlatma
Terminali .sln dosyasının bulunduğu klasörde açın:
```powershell
dotnet restore AutomotiveServiceManagement.sln
dotnet build AutomotiveServiceManagement.sln
dotnet run --project src/Web.UI --no-launch-profile --urls http://127.0.0.1:5180
```
Terminal açıkken tarayıcıda http://127.0.0.1:5180/ adresini açın. İlk açılışta SQLite veritabanı ve kurgusal örnek kayıtlar oluşturulur. Sonraki açılışta kayıtlar korunur. Kapatmak için terminalde Ctrl+C kullanın. İsteğe bağlı Start.ps1 aynı uygulamayı başlatır; terminal komutu PowerShell betik izinlerine bağlı değildir.

## 4. Visual Studio ile inceleme
.NET 10 projelerini destekleyen Visual Studio kullanın. File → Open → Project/Solution ile çıkartılmış klasördeki AutomotiveServiceManagement.sln dosyasını seçin. Web.UI projesini başlangıç projesi olarak ayarlayın. IDE profili farklı port açabilir; açılan tarayıcının adresini kullanın. Sabit 5180 adresi için yukarıdaki terminal komutu yeterlidir.

Tüm projelerde “not found” görünüyorsa .sln dosyası src/tests klasörlerinden ayrı açılmış olabilir. Solution'u kapatın ve ZIP'in tamamını çıkarttığınız ana klasördeki solution'u açın. Kaynak dosyalarını ayrı ayrı yeniden eklemeye gerek yoktur. SDK hatasında .NET SDK ve IDE desteğini kontrol edin.

## 5. Örnek inceleme akışı
Dashboard → Müşteriler → Araçlar → Servis kayıtları → Parça ve stok. Yeni müşteri/araç oluşturulabilir; araç detayından servis açılıp parçalar eklenebilir. Fiyat `125,50` veya `125.50` biçiminde girilir. Parça başlangıç stoğu sıfırdır; detayından stok girişi yapılır. İptal stok iadesi yapar; tamamlanan/iptal kayıt değişmez. Kılavuz: docs/KULLANIM.md.

## 6. Test
```powershell
dotnet run --project tests/IntegrationTests
```
Bu console test paketi gerçek SQLite kullanır; kullanıcı veritabanını değiştirmez. Opsiyonel HTTP testi: `python tools/http_smoke.py` (Python 3 gerektirir, 5181 portunu kullanır).

## Veri ve teslim sınırı
ZIP, yerel kullanımda eklenen müşteri/servis kayıtlarını içermez. Her temiz kurulum standart kurgusal örneklerle başlar; ekran görüntüleri mevcut geliştirme demosundan alındığı için sayılar farklı olabilir. Gerçek kullanım yedeği almak için uygulamayı kapatıp src/Web.UI/App_Data klasörünü ayrıca kopyalayın; kişisel veri yedeğini kaynak teslimi veya Claude paketine eklemeyin.

Responsive arayüz korunur. Ancak kaynak ZIP'i telefon veya tablette doğrudan çalıştırılmaz. Bu teslim için canlı yayın planlanmamıştır.
