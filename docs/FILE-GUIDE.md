# Dosya görev rehberi

Bir satır bir gerçek dosyaya karşılık gelir. bin/obj/yerel DB/önbellek bu pakette yoktur. Vendor dosyaları da envantere dahildir; uygulama kodundan ayrıdır.

| Dosya | Görev |
|---|---|
| `.gitignore` | Derleme, önbellek, yerel DB, secrets ve salt okunur referansları Git dışında tutar. |
| `AutomotiveServiceManagement.sln` | Tüm katmanları ve test projesini bir solution altında toplar. |
| `Directory.Build.props` | Projelerin ortak nullable, implicit using ve warnings-as-errors ayarları. |
| `docs/BILGISAYARDA-CALISTIRMA.md` | Nihai bilgisayar teslimi için ZIP çıkarma, SDK, Visual Studio/terminal açılışı, test ve veri sınırları. |
| `docs/CIHAZ-UYUMLULUGU.md` | 73 responsive yerleşim kontrolü, gerçek cihaz test sınırları ve hocanın ağdan erişim koşulları. |
| `docs/CLAUDE-REVIEW-BRIEF.md` | Claude için proje tanıtımı, inceleme kuralları ve beklenen rapor. |
| `docs/CLAUDE-REVIEW-DECISIONS.md` | Kullanıcının ilettiği bağımsız Claude raporunun kodla doğrulanması, uygulanan/uygulanmayan öneriler ve yeni test sonuçları. |
| `docs/ER-MODEL.md` | Mermaid ER ilişkileri ve para/concurrency saklama açıklaması. |
| `docs/FILE-GUIDE.md` | Bu paketteki her dosyanın tek tek görevi (bu rehber). |
| `docs/KULLANIM.md` | Türkçe kullanım, örnek veri, stok uyarısı ve hata giderme. |
| `docs/MASTER-SPECIFICATION-v1.md` | v1 kapsamı, gereksinimler, iş kuralları ve kabul ölçütleri. |
| `docs/SAVUNMA.md` | Öğrenci için OOP/DI/MVC/SQL/transaction açıklama ve demo notları. |
| `docs/VALIDATION.md` | Önceki build/test/görsel ve paket doğrulama sonuçları ile sınırlar. |
| `dotnet-tools.json` | Projeye yerel dotnet-ef aracının sabit sürümünü tanımlar. |
| `global.json` | .NET 10 SDK seçimi ve uyumlu SDK sürümüne ilerleme politikası. |
| `README.md` | Kurulum, modüller, mimari, veri güvenilirliği, test ve teslim açıklamaları. |
| `screenshots/customers.jpg` | Çalışan uygulamadan alınmış customers ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/dashboard.jpg` | Çalışan uygulamadan alınmış dashboard ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/mobile-dashboard.jpg` | Çalışan uygulamadan alınmış mobile-dashboard ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/mobile-form-validation.jpg` | Çalışan uygulamadan alınmış mobile-form-validation ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/README.md` | Ekran görüntülerinin içerik açıklamaları. |
| `screenshots/service-detail.jpg` | Çalışan uygulamadan alınmış service-detail ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/service-records.jpg` | Çalışan uygulamadan alınmış service-records ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/stock.jpg` | Çalışan uygulamadan alınmış stock ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/tablet-dashboard.jpg` | Çalışan uygulamadan alınmış tablet-dashboard ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/validation.jpg` | Çalışan uygulamadan alınmış validation ekran görüntüsü; uygulama kaynağı değildir. |
| `screenshots/vehicles.jpg` | Çalışan uygulamadan alınmış vehicles ekran görüntüsü; uygulama kaynağı değildir. |
| `src/Business/Business.csproj` | Bu katmanın hedef framework, NuGet paket ve diğer proje bağımlılıklarını tanımlar. |
| `src/Business/CustomerService.cs` | Müşteri arama, okuma, oluşturma/düzenleme, bağlı araç silme koruması. |
| `src/Business/DashboardService.cs` | LINQ ile müşteri/araç/açık servis sayıları, tamamlanan tutar, kritik stok ve son dosyalar. |
| `src/Business/PartService.cs` | Parça tanımı/fiyat, stok girişi ve stoklu/kullanılmış parça silme koruması. |
| `src/Business/ServiceBase.cs` | Ortak form/sürüm doğrulama, kuruşa dönüşüm, EF hata çevirisi ve sayfalama. |
| `src/Business/ServiceRecordService.cs` | Servis açma, stok düşerek parça ekleme, tamamlama/iptal ve atomik stok iadesi. |
| `src/Business/VehicleService.cs` | Plaka normalleştirme/benzersizlik, müşteri ilişkisi, kilometre ve geçmiş kuralları. |
| `src/Core/Core.csproj` | Bu katmanın hedef framework, NuGet paket ve diğer proje bağımlılıklarını tanımlar. |
| `src/Core/Entities.cs` | Entity tabanı/Version; Customer, Vehicle, Part, ServiceRecord, ServicePart ve durum enumu. Toplam ve Türkçe durum metni. |
| `src/Core/Forms.cs` | DataAnnotations ve IValidatableObject form doğrulaması; sayfalama/dashboard modelleri ve RuleException. |
| `src/Core/IServices.cs` | ICustomerService, IVehicleService, IPartService, IServiceRecordService, IDashboardService sözleşmeleri. |
| `src/Data/Data.csproj` | Bu katmanın hedef framework, NuGet paket ve diğer proje bağımlılıklarını tanımlar. |
| `src/Data/Migrations/20261016145144_InitialCreate.cs` | InitialCreate Up/Down: tabloları, ilişkileri, indeksleri ve kısıtları oluşturur/geri alır. |
| `src/Data/Migrations/20261016145144_InitialCreate.Designer.cs` | Başlangıç migration için EF tarafından üretilmiş hedef model metaverisi. |
| `src/Data/Migrations/ServiceDbContextModelSnapshot.cs` | EF Core modelinin son şema görünümü; sonraki migration farklarını hesaplamak için kullanılır. |
| `src/Data/SeedData.cs` | Kurgusal müşteri/araç/servis ve 14 parçalık katalog; eksik ek demo kodlarını tamamlama. |
| `src/Data/ServiceDbContext.cs` | EF tabloları, ilişkiler, benzersiz indeksler, CHECK ve concurrency ayarları; kayıtta Version yenileme. |
| `src/Web.UI/appsettings.Development.json` | Geliştirme ortamı logging ayarları. |
| `src/Web.UI/appsettings.json` | Logging ve host ayarları; API anahtarı/gerçek bağlantı içermez. |
| `src/Web.UI/Controllers/CustomersController.cs` | Müşteri liste/detay/form POST/silme ve kullanıcı bildirimleri. |
| `src/Web.UI/Controllers/HomeController.cs` | Dashboard, rehber, hata ve HTTP durum sayfaları. |
| `src/Web.UI/Controllers/PartsController.cs` | Parça liste/detay/form, stok girişi POST ve silme. |
| `src/Web.UI/Controllers/ServicesController.cs` | Servis liste/detay/açılış, parça ekleme ve durum değiştirme POST. |
| `src/Web.UI/Controllers/VehiclesController.cs` | Araç liste/detay/form, müşteri seçim listesi ve silme. |
| `src/Web.UI/Models/MoneyModelBinder.cs` | Virgül/nokta ondalıklı TL tutarlarını ayrıştırır; belirsiz/binlik girişleri reddeder; provider burada. |
| `src/Web.UI/Program.cs` | MVC, DI, SQLite, Türkçe kültür, para binder, global anti-forgery, migration/seed ve HTTP pipeline. |
| `src/Web.UI/Properties/launchSettings.json` | IDE geliştirme profilleri/portları; README komutu 5180 portunu açıkça seçer. |
| `src/Web.UI/Views/_ViewImports.cshtml` | Core namespace ve MVC tag helper içe aktarımları. |
| `src/Web.UI/Views/_ViewStart.cshtml` | Ortak Razor layout seçimi. |
| `src/Web.UI/Views/Customers/Details.cshtml` | Kayıt detayları ve ilgili işlemler; servis için parçalar/toplam/durum, parça için stok girişi. Modül: Customers. |
| `src/Web.UI/Views/Customers/Edit.cshtml` | Ekleme/düzenleme formu, hidden Id/Version ve doğrulama hataları. Modül: Customers. |
| `src/Web.UI/Views/Customers/Index.cshtml` | Liste, arama ve sayfalama görünümü. Modül: Customers. |
| `src/Web.UI/Views/Home/Help.cshtml` | Türkçe adım adım kullanım ve sorun giderme. |
| `src/Web.UI/Views/Home/Index.cshtml` | Dashboard: otomobil çizimli alan, gerçek metrikler, son servisler ve stok uyarıları. |
| `src/Web.UI/Views/Home/Message.cshtml` | Genel hata/404 gibi durumlar için mesaj görünümü. |
| `src/Web.UI/Views/Parts/Details.cshtml` | Kayıt detayları ve ilgili işlemler; servis için parçalar/toplam/durum, parça için stok girişi. Modül: Parts. |
| `src/Web.UI/Views/Parts/Edit.cshtml` | Ekleme/düzenleme formu, hidden Id/Version ve doğrulama hataları. Modül: Parts. |
| `src/Web.UI/Views/Parts/Index.cshtml` | Liste, arama ve sayfalama görünümü. Modül: Parts. |
| `src/Web.UI/Views/Services/Create.cshtml` | Yeni servis kabul formu ve doğrulama hataları. Modül: Services. |
| `src/Web.UI/Views/Services/Details.cshtml` | Kayıt detayları ve ilgili işlemler; servis için parçalar/toplam/durum, parça için stok girişi. Modül: Services. |
| `src/Web.UI/Views/Services/Index.cshtml` | Liste, arama ve sayfalama görünümü. Modül: Services. |
| `src/Web.UI/Views/Shared/_Icon.cshtml` | Menü/kartlar için dekoratif yerel SVG ikonları. |
| `src/Web.UI/Views/Shared/_Layout.cshtml` | Tüm sayfalarda menü, üst bar, bildirimler, CSS/JS sırası, footer. |
| `src/Web.UI/Views/Shared/_Pager.cshtml` | Sorguyu koruyan önceki/sonraki sayfa bağlantıları. |
| `src/Web.UI/Views/Shared/_Search.cshtml` | GET arama formu ve temizleme bağlantısı. |
| `src/Web.UI/Views/Shared/_ValidationScriptsPartial.cshtml` | Şablondan gelen jQuery validation referansları; mevcut formlarda çağrılmaz. Sunucu validation aktiftir. |
| `src/Web.UI/Views/Vehicles/Details.cshtml` | Kayıt detayları ve ilgili işlemler; servis için parçalar/toplam/durum, parça için stok girişi. Modül: Vehicles. |
| `src/Web.UI/Views/Vehicles/Edit.cshtml` | Ekleme/düzenleme formu, hidden Id/Version ve doğrulama hataları. Modül: Vehicles. |
| `src/Web.UI/Views/Vehicles/Index.cshtml` | Liste, arama ve sayfalama görünümü. Modül: Vehicles. |
| `src/Web.UI/Web.UI.csproj` | Bu katmanın hedef framework, NuGet paket ve diğer proje bağımlılıklarını tanımlar. |
| `src/Web.UI/wwwroot/css/premium.css` | site.css sonrasında tema, okunabilirlik, ikon/kart/hero ve responsive görünüm değişiklikleri. |
| `src/Web.UI/wwwroot/css/site.css` | İlk ortak yerleşim, tablolar, formlar, responsive temel stiller. |
| `src/Web.UI/wwwroot/favicon.ico` | Şablondan kalan tarayıcı sekme simgesi. |
| `src/Web.UI/wwwroot/images/workshop-car.svg` | Ön/arka ayrımı belirgin, marka/logosuz özgün vektör otomobil çizimi. |
| `src/Web.UI/wwwroot/js/site.js` | data-confirm bulunan formlarda işlem onayı; iptalde submit engelleme. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.min.css` | bootstrap yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.min.css.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.js` | bootstrap yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.js.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.min.js` | bootstrap yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.js` | bootstrap yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.js.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.min.js` | bootstrap yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.min.js.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.js` | bootstrap yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.js.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js` | bootstrap yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js.map` | bootstrap kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/bootstrap/LICENSE` | bootstrap üçüncü taraf lisans metni; proje iş mantığı değildir. |
| `src/Web.UI/wwwroot/lib/jquery/dist/jquery.js` | jquery yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery/dist/jquery.min.js` | jquery yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery/dist/jquery.min.map` | jquery kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/jquery/dist/jquery.slim.js` | jquery yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery/dist/jquery.slim.min.js` | jquery yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery/dist/jquery.slim.min.map` | jquery kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi. |
| `src/Web.UI/wwwroot/lib/jquery/LICENSE.txt` | jquery üçüncü taraf lisans metni; proje iş mantığı değildir. |
| `src/Web.UI/wwwroot/lib/jquery-validation/dist/additional-methods.js` | jquery-validation yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery-validation/dist/additional-methods.min.js` | jquery-validation yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery-validation/dist/jquery.validate.js` | jquery-validation yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery-validation/dist/jquery.validate.min.js` | jquery-validation yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery-validation/LICENSE.md` | jquery-validation üçüncü taraf lisans metni; proje iş mantığı değildir. |
| `src/Web.UI/wwwroot/lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js` | jquery-validation-unobtrusive yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js` | jquery-validation-unobtrusive yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez. |
| `src/Web.UI/wwwroot/lib/jquery-validation-unobtrusive/LICENSE.txt` | jquery-validation-unobtrusive üçüncü taraf lisans metni; proje iş mantığı değildir. |
| `Start.ps1` | Yerel MVC uygulamasını 5180 portunda başlatır. |
| `Test.ps1` | Solution build ve console entegrasyon senaryolarını çalıştırır. |
| `tests/IntegrationTests/IntegrationTests.csproj` | Bu katmanın hedef framework, NuGet paket ve diğer proje bağımlılıklarını tanımlar. |
| `tests/IntegrationTests/Scenarios.cs` | Gerçek SQLite üzerinde iş kuralları, migration, seed, stok, para, FK/CHECK ve concurrency test senaryoları. |
| `tools/http_smoke.py` | Geçici DB ve ayrı süreçle gerçek GET/POST/CSRF/form/para/stok HTTP kontrolleri. |
| `tools/package_source.py` | Allowlist ile kaynak teslim ZIP ve öğrenci adını içeren ayrı screenshot üretimi. |
| `tools/prepare_claude_review.py` | Dosya rehberi, okunabilir kaynak metni ve hash manifestli inceleme paketi üretimi. |
