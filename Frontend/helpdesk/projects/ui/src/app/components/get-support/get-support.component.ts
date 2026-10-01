import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-get-support',
  standalone: false,
  templateUrl: './get-support.component.html',
  styleUrl: './get-support.component.scss'
})
export class GetSupportComponent {
  private readonly formBuilder = inject(FormBuilder);

  readonly supportForm = this.formBuilder.nonNullable.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    category: ['', Validators.required],
    priority: ['normal', Validators.required],
    subject: ['', Validators.required],
    message: ['', [Validators.required, Validators.minLength(20)]]
  });

  submitted = false;
  attemptedSubmit = false;

  submitRequest(): void {
    this.attemptedSubmit = true;

    if (this.supportForm.invalid) {
      this.supportForm.markAllAsTouched();
      return;
    }

    this.submitted = true;
  }
}
