# Automotive Service Management System — v1

16 Ekim 2026. Hedef kullanıcı: tek bilgisayarda çalışan servis danışmanı.

## Resmi kaynak ve kapsam
MCSD.pdf incelendi: konu seçimi serbesttir; web ve WinForms türleri örneklenmiştir. Bu proje web tabanlı servis CRM ve stok yönetimidir. Örneklerde CRM/stok WinForms altında yer aldığından kategori yorumunun nihai kabulü eğitmene aittir; metin bir web CRM uygulamasını açıkça yasaklamamaktadır. Tam puan bir garanti değildir.

Zorunlular: database, OOP, HTML/CSS; sıkıştırılmış kaynak ve ayrıca ekran görüntüsü. İki dosyanın adında da öğrenci adı ve soyadı bulunmalıdır. PDF kaynak dosyası değiştirilmez.

## Teknoloji ve katmanlar
.NET 10, ASP.NET Core MVC, C#, EF Core, SQLite (ilişkisel SQL), Razor ve yerel Bootstrap. Core: varlıklar, form modelleri, servis arayüzleri. Data: DbContext, ilişkiler, migration, örnek veriler. Business: kurallar, async EF sorguları ve işlemler. Web.UI: controller, Razor, Türkçe yardım ve CSS. Testler gerçek SQLite kullanır. CQRS, MediatR ve genel repository yoktur.

## İşlev ve kabul ölçütleri
1. Müşteri: arama, liste, detay, ekleme, düzenleme, silme. Ad, telefon zorunlu; e-posta biçimi doğrulanır. Aracı olan müşteri silinmez.
2. Araç: müşteri seçimi, plaka, marka/model, yıl ve kilometre; CRUD ve servis geçmişi. Plaka normalize edilir, benzersizdir. Servis kaydı olan araç silinmez.
3. Servis: araç, tarih, kilometre, şikayet/işlem, işçilik; açık/tamamlandı/iptal. Gelecek tarih ve araç kilometresinden düşük yeni kayıt reddedilir. Tamamlanan/iptal servis değişmez. Silme yerine iptal, tarihçeyi korur.
4. Parça: kod, ad, kuruş cinsinden fiyat, stok ve kritik eşik. Kod benzersizdir. Stok girişi ayrı işlemdir; negatif stok imkansızdır. Servise parça ekleme atomik stok düşer; iptal tek seferde stok iade eder. Tarihi fiyat servis satırında saklanır; toplam sunucuda hesaplanır. Kullanılmış parça silinmez.
5. Dashboard: müşteri/araç sayısı, açık servis, tamamlanan servis tutarı ve kritik stok. İptaller tutara dahil edilmez. Boş durumlarda sonraki adım gösterilir.

## Güvenilirlik
Tüm değişiklikler POST + anti-forgery. Form modelleri overposting önler. İlişkisel FK, benzersiz indeks, CHECK kısıtları, transaction ve optimistic concurrency uygulanır. Razor varsayılan HTML encoding kullanır. Hatalı id 404; iş kuralı ihlali Türkçe form hatası; başarılı POST redirect. Arama ve tablolar sınırlı sayfalanır. Demo yerel kullanım içindir; internet yayını için kimlik doğrulama ayrıca gereklidir.

## Aşamalar
İskelet build → model/veritabanı build/test → müşteri build/test → araç build/test → servis/stok build/test → dashboard build/test → HTTP ve ekran denetimi → README/kullanım/teslim ZIP. Sonuçlar docs/VALIDATION.md içinde tutulur.

## Teslim
Temiz ortamda restore/build/test/run; deterministik örnek veri; screenshots/; kurulum ve Türkçe kullanım kılavuzu; ER modeli; kaynak ZIP. bin/obj, gerçek veri ve secrets ZIP/Git dışında. Öğrenci adı bilinmiyorsa final teslim adlandırması bekler. GitHub yayını ve e-posta gönderimi bu geliştirme adımının parçası değildir.
