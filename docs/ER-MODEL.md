# İlişkisel model

```mermaid
erDiagram
    Customer ||--o{ Vehicle : owns
    Vehicle ||--o{ ServiceRecord : history
    ServiceRecord ||--o{ ServicePart : uses
    Part ||--o{ ServicePart : supplies
    Customer {
        int Id PK
        string Name
        string Phone
        string Email
    }
    Vehicle {
        int Id PK
        int CustomerId FK
        string Plate UK
        string Brand
        string Model
        int Year
        int Mileage
    }
    ServiceRecord {
        int Id PK
        int VehicleId FK
        datetime Date
        int Mileage
        string Description
        int LaborCents
        int Status
    }
    Part {
        int Id PK
        string Code UK
        string Name
        int PriceCents
        int Stock
        int MinimumStock
    }
    ServicePart {
        int Id PK
        int ServiceRecordId FK
        int PartId FK
        int Quantity
        int UnitPriceCents
    }
```

Tüm varlıklarda Guid Version eşzamanlılık işaretidir. Para int kuruş olarak saklanır; kayan nokta hatası oluşmaz. İlişkiler Restrict ile korunur. Servis toplamı işçilik + miktar × o tarihteki fiyat toplamıdır. Stok değişimi ve servis durumu tek transaction içinde kaydedilir. Negatif tutar/stok ve sıfır miktar CHECK ile engellenir.
