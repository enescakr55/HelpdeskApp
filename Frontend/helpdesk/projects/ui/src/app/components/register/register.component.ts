import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DepartmentResponseModel } from '../../models/responses/departmentResponseModel';
import { AuthService } from '../../services/auth.service';
import { DepartmentService } from '../../services/department.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  readonly registerForm: FormGroup;
  departments: DepartmentResponseModel[] = [];
  isLoadingDepartments = true;
  isSubmitting = false;
  showPassword = false;
  statusMessage = '';
  statusType: 'success' | 'error' | '' = '';

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly departmentService: DepartmentService
  ) {
    this.registerForm = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      departmentId: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.departmentService.listDepartments().subscribe({
      next: (response) => {
        this.isLoadingDepartments = false;
        if (response.success && response.data) {
          this.departments = response.data;
        } else {
          this.showError(response.message || 'Departmanlar yüklenemedi.');
        }
      },
      error: (error) => {
        this.isLoadingDepartments = false;
        this.showError(error?.error?.message || 'Departmanlar yüklenirken bir hata oluştu.');
      }
    });
  }

  get passwordsDoNotMatch(): boolean {
    const password = this.registerForm.controls['password'].value;
    const confirmation = this.registerForm.controls['confirmPassword'].value;
    return Boolean(confirmation) && password !== confirmation;
  }

  submitRegistration(): void {
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid || this.passwordsDoNotMatch || this.isLoadingDepartments) {
      return;
    }

    const { firstname, lastname, email, departmentId, password } = this.registerForm.getRawValue();
    this.isSubmitting = true;
    this.statusMessage = '';
    this.statusType = '';

    this.authService.register({ firstname, lastname, email, departmentId, password }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        if (!response.success || !response.data) {
          this.showError(response.message || 'Kayıt oluşturulamadı. Bilgilerinizi kontrol edin.');
          return;
        }

        this.statusType = 'success';
        this.statusMessage = response.data.isActive
          ? 'Yönetici hesabınız oluşturuldu. Yönetici giriş sayfasından giriş yapabilirsiniz.'
          : 'Yönetici başvurunuz alındı. Hesabınız onaylandıktan sonra giriş yapabilirsiniz.';
        this.registerForm.reset();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.showError(
          error?.error?.message ||
          error?.error?.Message ||
          'Kayıt oluşturulamadı. Bilgilerinizi kontrol edip tekrar deneyin.'
        );
      }
    });
  }

  private showError(message: string): void {
    this.statusType = 'error';
    this.statusMessage = message;
  }
}