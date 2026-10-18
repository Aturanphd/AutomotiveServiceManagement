# Kodu anlatma ve demo notları

1. Amaç: servisin müşteri, araç, işlem ve stok ilişkilerini tek yerde izlemek.
2. OOP: Entity taban sınıfından türeyen Customer/Vehicle/Part; IServiceRecordService arayüzünün ServiceRecordService ile uygulanması. Kalıtım ve interface kullanımını bu dosyalarda gösterin.
3. DI: Program.cs servis kayıtları ve controller constructor parametreleri. Controller veri erişimini doğrudan yapmaz.
4. MVC: CustomerForm kullanıcı girişidir, CustomersController akışı yönetir, Edit.cshtml HTML formunu üretir. Domain entity ile form modelinin farkını açıklayın.
5. SQL: ER ilişkilerini ve Restrict silme kuralını gösterin. EF migration şemayı tekrar üretir. SQLite bir ilişkisel SQL veritabanıdır; ayrı SQL Server kurulumu gerektirmez.
6. LINQ/async: ListAsync filtre/sıralama/sayfalama; DashboardService toplam ve kritik stok sorguları. async/await veritabanı beklerken iş parçacığını gereksiz meşgul etmez.
7. Validation: DataAnnotations biçim/sınırları; service katmanı stok, kilometre, aynı plaka gibi iş kurallarını denetler. HTML kontrolü tek başına yeterli değildir.
8. Güvenli POST: form tag helper token üretir; global anti-forgery filtresi doğrular. GET yalnızca okur; form modelleri overposting riskini azaltır.
9. Transaction: AddPartAsync stok düşer ve servis satırını birlikte kaydeder. İptal stok iadesi ve durum değişikliğini birlikte kaydeder. Kısmi kayıt olmaz.
10. Eşzamanlılık: Version eski sayfanın yeni kaydı ezmesini önler. Para neden kuruş? Kayan nokta yuvarlama hatasını önlemek. Satır fiyatı neden ayrı? Tarihi fiyatı korumak.

## 5 dakikalık demo
Dashboard → müşteri ekle → araç ekle → parça/stoğa giriş → servis aç → parça ekle → toplamı göster → tamamla. Sonra aynı plaka, düşük kilometre, yetersiz stok ve bağlı müşteri silme hatalarını gösterin. İkinci bir servis iptal ederek stoğun geri geldiğini gösterin. Entegrasyon testini çalıştırın.

## Sınırlar
Bu sürüm yerel eğitim demosudur; kullanıcı girişi/rol, faturalama, vergi, tahsilat ve çok şubeli çalışma yoktur. Tam puan veya resmi kategori kabulü garanti edilmez. Konu seçiminin serbest olduğu kaynak metne rağmen kategori yorumunu eğitmen yapar. Kullanıcı Claude'un bağımsız kaynak okuma raporunu iletti; Claude uygulamayı çalıştırmadı. Rapor sonrası kararlar docs/CLAUDE-REVIEW-DECISIONS.md içindedir.
