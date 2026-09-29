# TransportApp - Düzce Akçakoca Servis Takip Sistemi

ASP.NET Core MVC ile geliştirilmiş, Düzce-Akçakoca arası servisleri takip eden web uygulaması.

## Özellikler

- **Taksi, Belediye Servisi, Minibüs** takibi
- **REST API** (Swagger dokümantasyonu ile)
- **Admin Panel** (CRUD işlemleri)
- **Kayıt & Giriş** (ASP.NET Core Identity)
- **Harita** entegrasyonu (OpenStreetMap)
- **Dark Mode** desteği
- **Arama & Filtreleme**

## Teknolojiler

| Teknoloji | Versiyon |
|-----------|----------|
| .NET | 8.0 |
| ASP.NET Core MVC | 8.0 |
| Entity Framework Core | 8.0 |
| SQLite | - |
| Tailwind CSS | 3.4 |
| Swagger | 6.5 |

## Kurulum

```bash
git clone https://github.com/KULLANICI_ADIN/TransportApp.git
cd TransportApp
dotnet restore
dotnet run
```

## URL'ler

| Sayfa | URL |
|-------|-----|
| Ana Sayfa | http://localhost:5000 |
| Servisler | http://localhost:5000/Transport |
| API Dokümantasyonu | http://localhost:5000/swagger |
| Admin Panel | http://localhost:5000/Admin |

## Admin Girişi

- **Email:** admin@transport.com
- **Şifre:** Admin123!

## API Endpoint'leri

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /api/transport | Tüm servisler |
| GET | /api/transport/{id} | Servis detayı |
| POST | /api/transport | Yeni servis |
| PUT | /api/transport/{id} | Güncelle |
| DELETE | /api/transport/{id} | Sil |
| GET | /api/stats | İstatistikler |

## Proje Yapısı

```
TransportApp/
├── Controllers/
│   ├── Api/
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── HomeController.cs
│   └── TransportController.cs
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── ApplicationUser.cs
│   ├── ContactMessage.cs
│   └── RouteInfo.cs
└── Views/
    ├── Account/
    ├── Admin/
    ├── Home/
    ├── Shared/
    └── Transport/
```

## Lisans

MIT License
