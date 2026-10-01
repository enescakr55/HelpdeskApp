import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SupportRequestService } from '../../services/support-request.service';

@Component({
  selector: 'app-get-support',
  standalone: false,
  templateUrl: './get-support.component.html',
  styleUrl: './get-support.component.scss'
})
export class GetSupportComponent {
  private readonly formBuilder: FormBuilder;
  private readonly supportRequestService: SupportRequestService;

  supportForm: FormGroup;
  submitted = false;
  attemptedSubmit = false;
  isSubmitting = false;
  statusMessage = '';

  constructor(
    formBuilder: FormBuilder,
    supportRequestService: SupportRequestService
  ) {
    this.formBuilder = formBuilder;
    this.supportRequestService = supportRequestService;

    this.supportForm = this.formBuilder.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      priority: ['normal', Validators.required],
      title: ['', Validators.required],
      description: ['', [Validators.required, Validators.minLength(20)]]
    });
  }

  submitRequest(): void {
    this.attemptedSubmit = true;

    if (this.supportForm.invalid) {
      this.supportForm.markAllAsTouched();
      return;
    }

    const priorityMap: Record<string, number> = {
      low: 1,
      normal: 2,
      high: 3
    };

    const payload = {
      fullname: this.supportForm.controls['fullName'].value,
      email: this.supportForm.controls['email'].value,
      subject: 1,
      priority: priorityMap[this.supportForm.controls['priority'].value] ?? 2,
      title: this.supportForm.controls['title'].value,
      description: this.supportForm.controls['description'].value
    };
    this.submitted = false;
    this.isSubmitting = true;
    this.statusMessage = '';

    this.supportRequestService.create(payload).subscribe({
      next: (response) => {
        console.log(response);
        this.isSubmitting = false;
        this.submitted = true;
        let requestCode = response?.data?.requestCode
        this.statusMessage = requestCode != null
          ? `Talebiniz başarıyla oluşturuldu. Talep kodunuz: ${requestCode}`
          : response?.message || 'Talebiniz başarıyla oluşturuldu.';
        this.supportForm.reset({
          priority: 'normal',
          description: ' '
        });
        this.attemptedSubmit = false;
      },
      error: (error) => {
        this.isSubmitting = false;
        this.submitted = false;
        this.statusMessage = error?.error?.message || 'Talep oluşturulurken bir hata oluştu.';
      }
    });
  }
}
