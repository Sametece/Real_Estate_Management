# Real Estate Management API

Real Estate Management API, emlak ilanlarını yönetmek, aramak, filtrelemek ve müşteriler ile emlakçılar arasında iletişim kurmak için geliştirilmiş bir RESTful API'dir.

## 📋 Proje Açıklaması

Bu proje, emlak sektörü için kapsamlı bir yönetim sistemi sunar. Emlakçılar ilanlarını yönetebilir, müşteriler emlak arayabilir ve sorgu gönderebilir, yöneticiler ise tüm sistemi kontrol edebilir.

## 🛠️ Teknoloji Stack

- **.NET 10.0** - Framework
- **ASP.NET Core Web API** - Web API framework
- **Entity Framework Core** - ORM
- **SQLite** - Database (Development için)
- **ASP.NET Core Identity** - Authentication & Authorization
- **JWT Bearer Authentication** - Token-based authentication
- **FluentValidation** - Model validation
- **AutoMapper** - Object mapping
- **Memory Caching** - Performance optimization
- **Swagger/OpenAPI** - API documentation

## 🏗️ Mimari

Proje 5 katmanlı mimari kullanmaktadır:

```
RealEstate.API (Presentation Layer)
    ↓
RealEstate.Business (Business Logic Layer)
    ↓
RealEstate.Data (Data Access Layer)
    ↓
RealEstate.Entity (Domain Models)
    ↓
RealEstate.Shared (Shared Utilities)
```

## 🚀 Kurulum

### Gereksinimler

Projeyi çalıştırmak için aşağıdaki yazılımların yüklü olması gerekir:

- .NET 10.0 SDK veya üzeri
- Git
- Visual Studio 2022 veya Visual Studio Code (opsiyonel)

### Adımlar

1. **Repository'yi clone'layın:**
   ```bash
   git clone https://github.com/kullaniciadi/real-estate-api.git
   cd real-estate-api
   ```

2. **NuGet paketlerini restore edin:**
   ```bash
   dotnet restore
   ```

3. **Database'i oluşturun:**
   ```bash
   cd RealEstate.API
   dotnet ef database update
   ```

4. **Uygulamayı çalıştırın:**
   ```bash
   dotnet run
   ```

5. **API'ye erişin:**
   - Swagger UI: `https://localhost:5070/swagger`
   - API Base URL: `https://localhost:5070/api`

## ⚙️ Environment Variables

Production ortamında aşağıdaki environment variable'ları ayarlamanız gerekir:

| Variable | Açıklama | Örnek Değer |
|----------|----------|-------------|
| `ConnectionStrings__SqliteConnection` | Database connection string | `Data Source=RealEstate.db` |
| `JwtConfig__Secret` | JWT şifreleme anahtarı | `your-secret-key-here-min-32-characters` |
| `JwtConfig__Issuer` | JWT issuer | `RealEstate_Backend` |
| `JwtConfig__Audience` | JWT audience | `RealEstate_Web` |
| `JwtConfig__AccessTokenExpiration` | Access token süresi (dakika) | `30` |
| `JwtConfig__RefreshTokenExpiration` | Refresh token süresi (gün) | `7` |

## 👥 Kullanıcı Rolleri

### Admin (Yönetici)
- Tüm sistem üzerinde tam yetki
- Kullanıcı yönetimi
- Emlak tipi yönetimi
- Tüm ilanları görüntüleme/düzenleme

### Agent (Emlakçı)
- Kendi ilanlarını oluşturma/düzenleme/silme
- Kendi ilanlarına gelen sorguları görüntüleme
- Sorgu durumlarını güncelleme
- Tüm ilanları görüntüleme (arama için)

### User (Kullanıcı)
- Tüm ilanları görüntüleme
- Arama ve filtreleme yapma
- Sorgu gönderme
- Kendi profilini görüntüleme/güncelleme

## 🔐 Test Kullanıcıları

Sistem aşağıdaki test kullanıcıları ile birlikte gelir:

| Rol | Email | Şifre |
|-----|-------|-------|
| Admin | admin@test.com | Admin123! |
| Agent | agent@test.com | Agent123! |
| User | user@test.com | User123! |

## 📚 API Endpoints

### Authentication
- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/login` - Giriş yap
- `POST /api/auth/refresh-token` - Token yenile
- `POST /api/auth/logout` - Çıkış yap
- `POST /api/auth/change-password` - Şifre değiştir
- `POST /api/auth/forgot-password` - Şifre sıfırlama isteği
- `POST /api/auth/reset-password` - Şifre sıfırla

### Properties (Emlak İlanları)
- `GET /api/properties` - Tüm ilanları listele (filtreleme, pagination)
- `GET /api/properties/{id}` - İlan detayı
- `POST /api/properties` - Yeni ilan oluştur (Agent/Admin)
- `PUT /api/properties/admin/{id}` - İlanı güncelle (Admin)
- `PUT /api/properties/agent/{id}` - İlanı güncelle (Agent)
- `DELETE /api/properties/{id}` - İlanı sil (Soft delete)
- `DELETE /api/properties/hard/{id}` - İlanı kalıcı sil

### Property Types (Emlak Türleri)
- `GET /api/propertytypes` - Tüm emlak tiplerini listele
- `GET /api/propertytypes/{id}` - Emlak tipi detayı
- `POST /api/propertytypes` - Yeni emlak tipi oluştur (Admin)
- `PUT /api/propertytypes/{id}` - Emlak tipini güncelle (Admin)
- `DELETE /api/propertytypes/{id}` - Emlak tipini sil (Admin)

### Property Images (Emlak Resimleri)
- `GET /api/propertyimages` - Resimleri listele
- `GET /api/propertyimages/property/{propertyId}` - İlana ait resimleri getir
- `POST /api/propertyimages` - Resim ekle
- `PUT /api/propertyimages/{id}` - Resmi güncelle
- `PUT /api/propertyimages/{id}/set-primary` - Ana resim yap
- `DELETE /api/propertyimages/{id}` - Resmi sil

### Inquiries (İletişim Mesajları)
- `GET /api/inquiries` - Sorguları listele (Agent/Admin)
- `GET /api/inquiries/{id}` - Sorgu detayı
- `POST /api/inquiries` - Yeni sorgu gönder
- `PUT /api/inquiries` - Sorgu durumunu güncelle (Agent/Admin)
- `DELETE /api/inquiries/{id}` - Sorguyu sil

### Users (Kullanıcılar)
- `GET /api/users/me` - Kendi profil bilgileri
- `PUT /api/users/me` - Profili güncelle
- `PUT /api/users/me/agent-info` - Emlakçı bilgilerini güncelle (Agent)
- `GET /api/users` - Tüm kullanıcıları listele (Admin)
- `GET /api/users/{id}` - Kullanıcı detayı (Admin)
- `PUT /api/users/{id}/role` - Kullanıcı rolünü güncelle (Admin)
- `DELETE /api/users/{id}` - Kullanıcıyı sil (Admin)

## 🔍 Arama ve Filtreleme

Properties endpoint'i aşağıdaki filtreleme parametrelerini destekler:

| Parametre | Tip | Açıklama |
|-----------|-----|----------|
| `pageNumber` | int | Sayfa numarası (varsayılan: 1) |
| `pageSize` | int | Sayfa boyutu (varsayılan: 10) |
| `minPrice` | decimal | Minimum fiyat |
| `maxPrice` | decimal | Maksimum fiyat |
| `city` | string | Şehir |
| `district` | string | İlçe |
| `minRooms` | int | Minimum oda sayısı |
| `maxRooms` | int | Maksimum oda sayısı |
| `minArea` | decimal | Minimum alan (m²) |
| `maxArea` | decimal | Maksimum alan (m²) |
| `propertyTypeId` | int | Emlak tipi ID |
| `status` | string | İlan durumu |
| `agentId` | int | Emlakçı ID |
| `minYear` | int | Minimum yapım yılı |
| `maxYear` | int | Maksimum yapım yılı |
| `sortBy` | string | Sıralama alanı (price, area, rooms, createdAt) |
| `sortOrder` | string | Sıralama yönü (asc, desc) |
| `searchTerm` | string | Arama terimi |

**Örnek Kullanım:**
```
GET /api/properties?city=İstanbul&minPrice=100000&maxPrice=500000&minRooms=2&sortBy=price&sortOrder=asc&pageNumber=1&pageSize=10
```

## 🔒 Authentication

API, JWT Bearer token authentication kullanır:

1. **Login:** `POST /api/auth/login` endpoint'ine email ve şifre gönderin
2. **Token Al:** Response'dan `accessToken` ve `refreshToken` alın
3. **Authorization Header:** Diğer isteklerde `Authorization: Bearer {accessToken}` header'ını kullanın
4. **Token Yenile:** Access token süresi dolduğunda `POST /api/auth/refresh-token` ile yenileyin

**Örnek Authorization Header:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📝 Validation Kuralları

### Property (Emlak İlanı)
- **Title:** 3-200 karakter arası, zorunlu
- **Description:** 10-5000 karakter arası, zorunlu
- **Price:** 0'dan büyük, maksimum 999,999,999
- **Address:** 5-500 karakter arası, zorunlu
- **City:** 2-100 karakter arası, zorunlu
- **Rooms:** 1-20 arası, zorunlu
- **Area:** 0'dan büyük, maksimum 100,000 m²
- **YearBuilt:** 1900-2100 arası

### User Registration
- **Email:** Geçerli email formatı, zorunlu, benzersiz
- **Password:** En az 8 karakter, büyük harf, küçük harf, rakam ve özel karakter içermeli
- **FirstName/LastName:** 2-50 karakter arası, zorunlu

### Inquiry (Sorgu)
- **Name:** 2-100 karakter arası, zorunlu
- **Email:** Geçerli email formatı, zorunlu
- **Message:** 10-1000 karakter arası, zorunlu

## 🚀 Özellikler

### ✅ Temel Özellikler
- **CRUD İşlemleri:** Tüm entity'ler için tam CRUD desteği
- **Authentication & Authorization:** JWT tabanlı güvenlik
- **Role-based Access Control:** Admin, Agent, User rolleri
- **Soft Delete:** Güvenli silme işlemleri
- **Global Exception Handling:** Merkezi hata yönetimi
- **FluentValidation:** Kapsamlı model doğrulama
- **AutoMapper:** Otomatik object mapping

### ✅ Gelişmiş Özellikler
- **Pagination:** Tüm liste endpoint'lerinde sayfalama
- **Filtering & Sorting:** Gelişmiş arama ve sıralama
- **Caching:** Memory cache ile performans optimizasyonu
- **Response Compression:** Gzip sıkıştırma
- **Security Headers:** Güvenlik başlıkları
- **CORS Support:** Cross-origin resource sharing
- **Swagger Documentation:** Otomatik API dokümantasyonu

### ✅ Güvenlik
- **JWT Authentication:** Access + Refresh token
- **Password Hashing:** BCrypt ile şifre hashleme
- **Role-based Authorization:** Endpoint bazlı yetkilendirme
- **Input Validation:** FluentValidation ile güvenli input
- **SQL Injection Protection:** Entity Framework Core koruması

## 🐛 Hata Yönetimi

API, tutarlı hata response formatı kullanır:

```json
{
  "data": null,
  "error": "Hata mesajı",
  "isSucceed": false,
  "statusCode": 400
}
```

### HTTP Status Codes
- **200:** Başarılı
- **201:** Oluşturuldu
- **204:** İçerik yok (başarılı silme/güncelleme)
- **400:** Geçersiz istek
- **401:** Yetkisiz erişim
- **403:** Yasak
- **404:** Bulunamadı
- **409:** Çakışma
- **500:** Sunucu hatası

## 📊 Response Format

Tüm API response'ları tutarlı format kullanır:

**Başarılı Response:**
```json
{
  "data": { /* response data */ },
  "error": null,
  "isSucceed": true,
  "statusCode": 200
}
```

**Hatalı Response:**
```json
{
  "data": null,
  "error": "Hata mesajı",
  "isSucceed": false,
  "statusCode": 400
}
```

**Paginated Response:**
```json
{
  "data": {
    "data": [/* items */],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 100,
    "totalPages": 10,
    "hasPrevious": false,
    "hasNext": true
  },
  "error": null,
  "isSucceed": true,
  "statusCode": 200
}
```

## 🧪 Testing

### Postman Collection

API'yi test etmek için Postman collection'ı kullanabilirsiniz:

1. Postman'i açın
2. Collection import edin
3. Environment variables'ları ayarlayın:
   - `baseUrl`: `https://localhost:5070`
   - `accessToken`: Login sonrası otomatik set edilir

### Test Senaryoları

1. **Authentication Test:**
   - Register ile yeni kullanıcı oluştur
   - Login ile token al
   - Protected endpoint'leri test et

2. **Property Management:**
   - Agent olarak login ol
   - Yeni ilan oluştur
   - İlanı güncelle ve sil

3. **Search & Filter:**
   - Farklı filtrelerle arama yap
   - Pagination test et
   - Sorting test et

## 🔧 Development

### Database Migration

Yeni migration oluşturmak için:

```bash
cd RealEstate.API
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Yeni Entity Ekleme

1. `RealEstate.Entity/Concrete` klasörüne entity ekle
2. `RealEstateDbContext`'e DbSet ekle
3. Migration oluştur ve uygula
4. Service ve Controller oluştur

### Yeni Endpoint Ekleme

1. DTO'ları oluştur (`RealEstate.Business/DTOs`)
2. Validator ekle (`RealEstate.Business/Validators`)
3. Service interface ve implementation oluştur
4. Controller'a endpoint ekle
5. AutoMapper profile güncelle

## 📈 Performance

### Caching Strategy

- **PropertyTypes:** 30 dakika cache (değişmediği için)
- **Popular Properties:** 15 dakika cache
- **User Profiles:** 10 dakika cache

### Database Optimization

- **Indexes:** Sık kullanılan alanlar için index
- **Query Filters:** Soft delete için global filter
- **AsNoTracking:** Read-only sorgular için
- **Pagination:** Büyük veri setleri için sayfalama

## 🚀 Deployment

### Local Development

```bash
git clone <repository-url>
cd real-estate-api
dotnet restore
dotnet ef database update --project RealEstate.API
dotnet run --project RealEstate.API
```

### Production Deployment

1. **Environment Variables'ları ayarla**
2. **Connection string'i güncelle**
3. **JWT secret'ı güçlü bir değer yap**
4. **HTTPS kullan**
5. **Database migration'larını çalıştır**

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit yapın (`git commit -m 'Add some AmazingFeature'`)
4. Branch'i push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📞 İletişim

Proje hakkında sorularınız için:

- **Email:** your-email@example.com
- **GitHub:** [github.com/yourusername](https://github.com/yourusername)

---

**Real Estate Management API** - Emlak sektörü için modern, güvenli ve ölçeklenebilir API çözümü 🏠