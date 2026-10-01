# HelpdeskApp

Herhangi bir platform için kullanılabilecek destek taleplerinin oluşturulması ve yönetilmesi için geliştirilen web uygulaması.

## Teknolojiler

- Backend: .NET 8, ASP.NET Core, FastEndpoints, Entity Framework Core, PostgreSQL
- Frontend: Angular 19, TypeScript, Bootstrap, ngx-toastr

## Proje Özellikleri

 - Kullanıcılar herhangi bir üyeliğe ihtiyaç duymadan destek talebi oluşturabilirler.
 - Kullanıcılar kendine atanan kod ile destek talebinin durumunu görebilir.
 - Yöneticiler gelen bir destek talebini başka bir departmana yönlendirebilir
 - Yöneticiler destek talebinin durumunu değiştirebilir, yalnızca yöneticilerin göreceği bir mesaj ekleyebilir ya da kullanıcıyı bilgilendirmek için bir mesaj ekleyebilir
 - Oluşturulan ilk hesap yönetici hesabı olarak atanır, sonraki oluşturulan hesapların yönetici onayını almaları gerekir.
 - Yöneticiler yeni departman oluşturabilir, mevcut olan departmanı silebilir.

# Proje Görselleri

<img width="1920" height="1994" alt="screencapture-helpdesk-app-topaz-vercel-app-2026-10-01-20_32_00" src="https://github.com/user-attachments/assets/fa8f4b73-ff3b-4ead-965c-acaa871483cb" />


<img width="1920" height="1219" alt="screencapture-helpdesk-app-topaz-vercel-app-support-request-2026-10-01-20_31_32" src="https://github.com/user-attachments/assets/be4c3187-f207-4e87-a9c3-c26c0728eec8" />


<img width="1920" height="1129" alt="screencapture-helpdesk-app-topaz-vercel-app-support-track-2026-10-01-20_33_47" src="https://github.com/user-attachments/assets/e5344b2a-da22-44c9-af0f-ec0bd20c1c1c" />


<img width="1920" height="923" alt="screencapture-helpdesk-app-topaz-vercel-app-admin-login-2026-10-01-20_31_49" src="https://github.com/user-attachments/assets/96a7f6ef-d649-4c45-94b1-a05ad8a826c3" />


<img width="1920" height="923" alt="screencapture-helpdesk-app-topaz-vercel-app-register-2026-10-01-20_33_29" src="https://github.com/user-attachments/assets/b6cd5788-398f-46d1-a072-628be7850809" />


<img width="1920" height="1141" alt="screencapture-helpdesk-app-topaz-vercel-app-admin-support-requests-2026-10-01-20_32_25" src="https://github.com/user-attachments/assets/6f3856e1-9347-4733-9084-62968ca86a0d" />


<img width="1920" height="1141" alt="screencapture-helpdesk-app-topaz-vercel-app-admin-departments-2026-10-01-20_33_06" src="https://github.com/user-attachments/assets/5dd70a7d-9787-4627-bad8-7153a922926e" />


<img width="1920" height="1141" alt="screencapture-helpdesk-app-topaz-vercel-app-admin-users-2026-10-01-20_33_16" src="https://github.com/user-attachments/assets/8fad8bcb-452c-4f07-b669-022690199b56" />



















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

Frontend'i Başlatın

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



