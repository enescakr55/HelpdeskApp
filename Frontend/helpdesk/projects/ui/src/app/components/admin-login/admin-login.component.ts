import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-login',
  standalone: false,
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.scss'
})
export class AdminLoginComponent {
  readonly loginForm: FormGroup;
  isSubmitting = false;
  showPassword = false;
  statusMessage = '';
  statusType: 'success' | 'error' | '' = '';

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly toastr:ToastrService,
    private readonly router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  submitLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.statusMessage = '';
    this.statusType = '';

    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: (response) => {
        this.isSubmitting = false;

        if (!response.success || !response.data) {
          this.showError(response.message || 'Giriş başarısız. Bilgilerinizi kontrol edin.');
          return;
        }

        if (!response.data.isAdmin) {
          this.showError('Bu hesap yönetici yetkisine sahip değil.');
          return;
        }

        localStorage.setItem('authToken', response.data.token);
        localStorage.setItem('authRole', 'Admin');
        this.statusType = 'success';
        this.statusMessage = `Hoş geldiniz, ${response.data.firstname} ${response.data.lastname}. Yönetici oturumunuz açıldı.`;
        this.router.navigate(['/admin']);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.showError(
          error?.error?.message ||
          'Giriş yapılamadı. E-posta ve parolanızı kontrol edin.'
        );
      }
    });
  }

  private showError(message: string): void {
    this.statusType = 'error';
    this.statusMessage = message;
  }
}