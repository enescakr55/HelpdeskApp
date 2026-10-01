import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SupportRequestTrackResponseModel } from '../../models/responses/supportRequestTrackResponseModel';
import { SupportRequestService } from '../../services/support-request.service';

@Component({
  selector: 'app-track-request',
  standalone: false,
  templateUrl: './track-request.component.html',
  styleUrl: './track-request.component.scss'
})
export class TrackRequestComponent {
  readonly trackingForm: FormGroup;
  request: SupportRequestTrackResponseModel | null = null;
  isLoading = false;
  attemptedSubmit = false;
  statusMessage = '';

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly supportRequestService: SupportRequestService
  ) {
    this.trackingForm = this.formBuilder.group({
      requestCode: ['', Validators.required]
    });
  }

  findRequest(): void {
    this.attemptedSubmit = true;
    if (this.trackingForm.invalid || this.isLoading) {
      this.trackingForm.markAllAsTouched();
      return;
    }

    const requestCode = String(this.trackingForm.controls['requestCode'].value).trim().toUpperCase();
    this.request = null;
    this.statusMessage = '';
    this.isLoading = true;

    this.supportRequestService.getByRequestCode(requestCode).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.success && response.data) {
          this.request = response.data;
          return;
        }
        this.statusMessage = response.message || 'Bu kodla eşleşen bir destek talebi bulunamadı.';
      },
      error: (error) => {
        this.isLoading = false;
        this.statusMessage = error?.error?.message || error?.error?.Message || 'Talep bulunamadı. Kodunuzu kontrol edip tekrar deneyin.';
      }
    });
  }

  statusLabel(status: number): string {
    return [
      'Talep alındı',
      'İnceleniyor',
      'Yanıtınız bekleniyor',
      'Çözüldü',
      'Kapatıldı'
    ][status] || 'Durum bilgisi yok';
  }

  statusClass(status: number): string {
    return `request-status-${status}`;
  }

  priorityLabel(priority: number): string {
    return ['Düşük', 'Normal', 'Yüksek'][priority - 1] || 'Normal';
  }
}