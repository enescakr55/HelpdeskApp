# AI Log

Bu dosya, projede AI desteğiyle yapılan geliştirmeleri, alınan teknik kararları ve doğrulama durumunu kaydeder. Her oturumda yalnızca doğrulanmış değişiklikler yazılmalı; kullanıcıya ait sırlar veya kişisel veriler eklenmemelidir.

## Proje Bağlamı

- Backend: .NET 8, ASP.NET Core, FastEndpoints, Entity Framework Core ve PostgreSQL.
- Frontend: Angular 19, Bootstrap, Reactive Forms ve `ngx-toastr`.
- Temel akış: destek talebi oluşturma ve kodla takip, yönetici girişi, departman yönetimi, kullanıcı ve yönetici başvurularını yönetme.
- Katmanlar: Web API endpoint'leri -> Services -> DataAccess/EF Core.

## AI Destekli Çalışma Kaydı

### 2026-10-01 — Admin paneli ve destek takibi

- Kullanıcıya özel destek talebi takip response modeli eklendi. Takip yanıtı yalnızca `requestCode`, `subject`, `priority`, `title`, `description`, `status` ve `userMessage` alanlarını içeriyor.
- Talep koduyla arama servisi ve endpoint'i bu özel modele bağlandı; genel destek talebi response modeli değiştirilmedi.
- Frontend takip modeli ve takip sayfası aynı alanlara göre güncellendi.
- Yeni talep kodlarının rastgele bölümü 6 karakterden 16 hex karaktere çıkarıldı. Bu değişiklik gelecekte oluşturulacak kodlara uygulanır; mevcut kodlar değişmez.
- Admin workspace destek talepleri, departmanlar ve kullanıcılar için ayrı routed component'lere ayrıldı. Admin servis metotları ve kullanıcı listeleme endpoint'i eklendi.
- Yönetici başvuru onayı, auth interceptor, admin guard ve platform odaklı landing page geliştirildi.

