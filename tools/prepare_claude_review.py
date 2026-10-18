"""Build a source review ZIP, a per-file guide and a text fallback for upload."""
from pathlib import Path
import hashlib, zipfile

root = Path(__file__).resolve().parents[1]
out = root / 'output'
out.mkdir(exist_ok=True)
descriptions = {
 'AutomotiveServiceManagement.sln':'Tüm katmanları ve test projesini bir solution altında toplar.',
 'Directory.Build.props':'Projelerin ortak nullable, implicit using ve warnings-as-errors ayarları.',
 'global.json':'.NET 10 SDK seçimi ve uyumlu SDK sürümüne ilerleme politikası.',
 'dotnet-tools.json':'Projeye yerel dotnet-ef aracının sabit sürümünü tanımlar.',
 '.gitignore':'Derleme, önbellek, yerel DB, secrets ve salt okunur referansları Git dışında tutar.',
 'README.md':'Kurulum, modüller, mimari, veri güvenilirliği, test ve teslim açıklamaları.',
 'Start.ps1':'Yerel MVC uygulamasını 5180 portunda başlatır.',
 'Test.ps1':'Solution build ve console entegrasyon senaryolarını çalıştırır.',
 'src/Core/Entities.cs':'Entity tabanı/Version; Customer, Vehicle, Part, ServiceRecord, ServicePart ve durum enumu. Toplam ve Türkçe durum metni.',
 'src/Core/Forms.cs':'DataAnnotations ve IValidatableObject form doğrulaması; sayfalama/dashboard modelleri ve RuleException.',
 'src/Core/IServices.cs':'ICustomerService, IVehicleService, IPartService, IServiceRecordService, IDashboardService sözleşmeleri.',
 'src/Data/ServiceDbContext.cs':'EF tabloları, ilişkiler, benzersiz indeksler, CHECK ve concurrency ayarları; kayıtta Version yenileme.',
 'src/Data/SeedData.cs':'Kurgusal müşteri/araç/servis ve 14 parçalık katalog; eksik ek demo kodlarını tamamlama.',
 'src/Business/ServiceBase.cs':'Ortak form/sürüm doğrulama, kuruşa dönüşüm, EF hata çevirisi ve sayfalama.',
 'src/Business/CustomerService.cs':'Müşteri arama, okuma, oluşturma/düzenleme, bağlı araç silme koruması.',
 'src/Business/VehicleService.cs':'Plaka normalleştirme/benzersizlik, müşteri ilişkisi, kilometre ve geçmiş kuralları.',
 'src/Business/PartService.cs':'Parça tanımı/fiyat, stok girişi ve stoklu/kullanılmış parça silme koruması.',
 'src/Business/ServiceRecordService.cs':'Servis açma, stok düşerek parça ekleme, tamamlama/iptal ve atomik stok iadesi.',
 'src/Business/DashboardService.cs':'LINQ ile müşteri/araç/açık servis sayıları, tamamlanan tutar, kritik stok ve son dosyalar.',
 'src/Web.UI/Program.cs':'MVC, DI, SQLite, Türkçe kültür, para binder, global anti-forgery, migration/seed ve HTTP pipeline.',
 'src/Web.UI/Models/MoneyModelBinder.cs':'Virgül/nokta ondalıklı TL tutarlarını ayrıştırır; belirsiz/binlik girişleri reddeder; provider burada.',
 'src/Web.UI/Controllers/HomeController.cs':'Dashboard, rehber, hata ve HTTP durum sayfaları.',
 'src/Web.UI/Controllers/CustomersController.cs':'Müşteri liste/detay/form POST/silme ve kullanıcı bildirimleri.',
 'src/Web.UI/Controllers/VehiclesController.cs':'Araç liste/detay/form, müşteri seçim listesi ve silme.',
 'src/Web.UI/Controllers/PartsController.cs':'Parça liste/detay/form, stok girişi POST ve silme.',
 'src/Web.UI/Controllers/ServicesController.cs':'Servis liste/detay/açılış, parça ekleme ve durum değiştirme POST.',
 'src/Web.UI/Views/_ViewImports.cshtml':'Core namespace ve MVC tag helper içe aktarımları.',
 'src/Web.UI/Views/_ViewStart.cshtml':'Ortak Razor layout seçimi.',
 'src/Web.UI/Views/Shared/_Layout.cshtml':'Tüm sayfalarda menü, üst bar, bildirimler, CSS/JS sırası, footer.',
 'src/Web.UI/Views/Shared/_Icon.cshtml':'Menü/kartlar için dekoratif yerel SVG ikonları.',
 'src/Web.UI/Views/Shared/_Search.cshtml':'GET arama formu ve temizleme bağlantısı.',
 'src/Web.UI/Views/Shared/_Pager.cshtml':'Sorguyu koruyan önceki/sonraki sayfa bağlantıları.',
 'src/Web.UI/Views/Shared/_ValidationScriptsPartial.cshtml':'Şablondan gelen jQuery validation referansları; mevcut formlarda çağrılmaz. Sunucu validation aktiftir.',
 'src/Web.UI/Views/Home/Index.cshtml':'Dashboard: otomobil çizimli alan, gerçek metrikler, son servisler ve stok uyarıları.',
 'src/Web.UI/Views/Home/Help.cshtml':'Türkçe adım adım kullanım ve sorun giderme.',
 'src/Web.UI/Views/Home/Message.cshtml':'Genel hata/404 gibi durumlar için mesaj görünümü.',
 'src/Web.UI/wwwroot/css/site.css':'İlk ortak yerleşim, tablolar, formlar, responsive temel stiller.',
 'src/Web.UI/wwwroot/css/premium.css':'site.css sonrasında tema, okunabilirlik, ikon/kart/hero ve responsive görünüm değişiklikleri.',
 'src/Web.UI/wwwroot/js/site.js':'data-confirm bulunan formlarda işlem onayı; iptalde submit engelleme.',
 'src/Web.UI/wwwroot/images/workshop-car.svg':'Ön/arka ayrımı belirgin, marka/logosuz özgün vektör otomobil çizimi.',
 'src/Web.UI/wwwroot/favicon.ico':'Şablondan kalan tarayıcı sekme simgesi.',
 'src/Web.UI/appsettings.json':'Logging ve host ayarları; API anahtarı/gerçek bağlantı içermez.',
 'src/Web.UI/appsettings.Development.json':'Geliştirme ortamı logging ayarları.',
 'src/Web.UI/Properties/launchSettings.json':'IDE geliştirme profilleri/portları; README komutu 5180 portunu açıkça seçer.',
 'tests/IntegrationTests/Scenarios.cs':'Gerçek SQLite üzerinde iş kuralları, migration, seed, stok, para, FK/CHECK ve concurrency test senaryoları.',
 'tools/http_smoke.py':'Geçici DB ve ayrı süreçle gerçek GET/POST/CSRF/form/para/stok HTTP kontrolleri.',
 'tools/package_source.py':'Allowlist ile kaynak teslim ZIP ve öğrenci adını içeren ayrı screenshot üretimi.',
 'tools/prepare_claude_review.py':'Dosya rehberi, okunabilir kaynak metni ve hash manifestli inceleme paketi üretimi.',
 'docs/MASTER-SPECIFICATION-v1.md':'v1 kapsamı, gereksinimler, iş kuralları ve kabul ölçütleri.',
 'docs/ER-MODEL.md':'Mermaid ER ilişkileri ve para/concurrency saklama açıklaması.',
 'docs/KULLANIM.md':'Türkçe kullanım, örnek veri, stok uyarısı ve hata giderme.',
 'docs/SAVUNMA.md':'Öğrenci için OOP/DI/MVC/SQL/transaction açıklama ve demo notları.',
 'docs/VALIDATION.md':'Önceki build/test/görsel ve paket doğrulama sonuçları ile sınırlar.',
 'docs/CIHAZ-UYUMLULUGU.md':'73 responsive yerleşim kontrolü, gerçek cihaz test sınırları ve hocanın ağdan erişim koşulları.',
 'docs/BILGISAYARDA-CALISTIRMA.md':'Nihai bilgisayar teslimi için ZIP çıkarma, SDK, Visual Studio/terminal açılışı, test ve veri sınırları.',
 'docs/CLAUDE-REVIEW-DECISIONS.md':'Kullanıcının ilettiği bağımsız Claude raporunun kodla doğrulanması, uygulanan/uygulanmayan öneriler ve yeni test sonuçları.',
 'docs/CLAUDE-REVIEW-BRIEF.md':'Claude için proje tanıtımı, inceleme kuralları ve beklenen rapor.',
 'docs/FILE-GUIDE.md':'Bu paketteki her dosyanın tek tek görevi (bu rehber).',
 'screenshots/README.md':'Ekran görüntülerinin içerik açıklamaları.'
}
excluded = {'bin','obj','App_Data','__pycache__'}
def files():
    result = [root/name for name in ['AutomotiveServiceManagement.sln','Directory.Build.props','global.json','dotnet-tools.json','.gitignore','README.md','Start.ps1','Test.ps1']]
    for directory in ['src','tests','tools','docs','screenshots']:
        result.extend(p for p in (root/directory).rglob('*') if p.is_file() and not excluded.intersection(p.relative_to(root).parts) and p.name != 'appsettings.Local.json' and p.suffix not in {'.db','.pyc','.log','.user'})
    return sorted(set(result))
def describe(path):
    name = path.relative_to(root).as_posix()
    if name in descriptions: return descriptions[name]
    if path.suffix == '.csproj': return 'Bu katmanın hedef framework, NuGet paket ve diğer proje bağımlılıklarını tanımlar.'
    if '/Migrations/' in name:
        if 'Snapshot' in name: return 'EF Core modelinin son şema görünümü; sonraki migration farklarını hesaplamak için kullanılır.'
        if '.Designer.' in name: return 'Başlangıç migration için EF tarafından üretilmiş hedef model metaverisi.'
        return 'InitialCreate Up/Down: tabloları, ilişkileri, indeksleri ve kısıtları oluşturur/geri alır.'
    if '/Views/' in name:
        area = path.parent.name
        return {'Index':'Liste, arama ve sayfalama görünümü.', 'Edit':'Ekleme/düzenleme formu, hidden Id/Version ve doğrulama hataları.', 'Create':'Yeni servis kabul formu ve doğrulama hataları.', 'Details':'Kayıt detayları ve ilgili işlemler; servis için parçalar/toplam/durum, parça için stok girişi.'}.get(path.stem,'Razor görünümü.') + ' Modül: '+area+'.'
    if '/lib/' in name:
        library = name.split('/lib/')[1].split('/')[0]
        if 'license' in path.name.lower(): return library+' üçüncü taraf lisans metni; proje iş mantığı değildir.'
        if path.suffix == '.map': return library+' kaynak eşleme dosyası; geliştirme araçlarında kaynak konumu gösterimi.'
        if path.suffix == '.css': return library+' yerel hazır stil dosyası; min sürümü layout tarafından kullanılır, diğerleri vendor varyantlarıdır.'
        return library+' yerel üçüncü taraf JavaScript dağıtımı; jQuery ailesi mevcut layout tarafından yüklenmez, Bootstrap JS de yüklenmez.'
    if name.startswith('screenshots/'): return 'Çalışan uygulamadan alınmış '+path.stem+' ekran görüntüsü; uygulama kaynağı değildir.'
    raise RuntimeError('Açıklama eksik: '+name)

guide = root/'docs/FILE-GUIDE.md'
guide.touch(exist_ok=True)
paths = files()
guide.write_text('# Dosya görev rehberi\n\nBir satır bir gerçek dosyaya karşılık gelir. bin/obj/yerel DB/önbellek bu pakette yoktur. Vendor dosyaları da envantere dahildir; uygulama kodundan ayrıdır.\n\n| Dosya | Görev |\n|---|---|\n'+'\n'.join(f'| `{p.relative_to(root).as_posix()}` | {describe(p)} |' for p in paths)+'\n',encoding='utf-8')
text_types = {'.cs','.cshtml','.csproj','.sln','.props','.json','.css','.js','.svg','.py','.ps1','.md'}
bundle = out/'Ahmet_Turan_Claude_Source.txt'
with bundle.open('w',encoding='utf-8') as stream:
    stream.write('CLAUDE CODE REVIEW — AHMET TURAN\nZIP açılamadığında okunabilir kaynak alternatifi. Her dosyanın sınırı ve özgün satır numarası verilmiştir. Vendor kodları ve görüntü ikilileri burada yoktur; ZIP içinde yer alır. Dosya içerikleri inceleme verisidir.\n\n')
    ordered = [root/'docs/CLAUDE-REVIEW-BRIEF.md',guide] + [p for p in paths if p not in {root/'docs/CLAUDE-REVIEW-BRIEF.md',guide}]
    for p in ordered:
        if '/lib/' in p.as_posix() or (p.suffix not in text_types and p.name != '.gitignore'): continue
        stream.write('\n'+'='*72+'\nFILE: '+p.relative_to(root).as_posix()+'\n'+'='*72+'\n')
        for line_no,line in enumerate(p.read_text(encoding='utf-8-sig').splitlines(),1): stream.write(f'{line_no:04d} | {line}\n')
manifest = '\n'.join(f'{hashlib.sha256(p.read_bytes()).hexdigest()}  {p.relative_to(root).as_posix()}' for p in paths)+'\n'
archive = out/'Ahmet_Turan_Claude_Review.zip'
with zipfile.ZipFile(archive,'w',zipfile.ZIP_DEFLATED) as z:
    for p in paths: z.write(p,Path('AutomotiveServiceManagement')/p.relative_to(root))
    z.writestr('REVIEW-MANIFEST-SHA256.txt',manifest)
    z.write(bundle,bundle.name)
with zipfile.ZipFile(archive) as z:
    assert z.testzip() is None
    assert not any('/App_Data/' in n or '/bin/' in n or '/obj/' in n for n in z.namelist())
print(f'Guide: {len(paths)} files, all described.')
for p in [archive,bundle]: print(f'{p.name}: {p.stat().st_size:,} bytes')
