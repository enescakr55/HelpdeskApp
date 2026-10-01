# HelpdeskApp

Herhangi bir platform için kullanılabilecek destek taleplerinin oluşturulması ve yönetilmesi için geliştirilen web uygulaması.

## Teknolojiler

- Backend: .NET 8, ASP.NET Core, FastEndpoints, Entity Framework Core, PostgreSQL
- Frontend: Angular 19, TypeScript, Bootstrap, ngx-toastr

## Proje Yapısı

```text
Backend/Helpdesk/       .NET çözümü ve API
Frontend/helpdesk/      Angular uygulaması
AI_LOG.md               AI destekli geliştirme notları
```

## Gereksinimler

- .NET 8 SDK
- Node.js ve npm
- PostgreSQL

## Yerel Çalıştırma

Önce PostgreSQL bağlantı bilgisini ve JWT secret'ını yerel ortam için yapılandırın. Gerçek parola ve secret değerlerini kaynak koda veya Git'e eklemeyin.

Backend'i başlatın:

```powershell
cd Backend/Helpdesk
dotnet restore Helpdesk.sln
dotnet run --project Helpdesk.WebApi --launch-profile https
```



```powershell
cd Frontend/helpdesk
npm install
npm start
```

Angular uygulaması varsayılan olarak `http://localhost:4200` adresinde açılır. Frontend API adresi `projects/ui/src/environments/environment.ts` dosyasında tanımlıdır. Yerel API kullanacaksanız bu adresi yerel backend URL'siyle eşleştirin.

## Uygulama Sayfaları

- `/` — Platform destek landing page'i
- `/support/request` — Yeni destek talebi oluşturma
- `/support/track` — Talep koduyla durum ve mesaj takibi
- `/admin/login` — Yönetici girişi
- `/admin` — Yetkili yönetici paneli

