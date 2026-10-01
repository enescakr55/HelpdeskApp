import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DepartmentResponseModel } from '../../../models/responses/departmentResponseModel';
import { SupportRequestResponse } from '../../../models/requests/supportRequestResponse';
import { DepartmentService } from '../../../services/department.service';
import { SupportRequestService } from '../../../services/support-request.service';

@Component({
  selector: 'app-admin-support-requests',
  standalone: false,
  templateUrl: './support-requests.component.html',
  styleUrl: './support-requests.component.scss'
})
export class SupportRequestsComponent implements OnInit {
  loadingRequests = false;
  requestSearch = '';
  requests: SupportRequestResponse[] = [];
  departments: DepartmentResponseModel[] = [];
  showDetails: string | null = null;
  updateUserMessage: boolean = false;
  showTextarea:boolean = true;
  readonly statusOptions = [
    { value: 0, label: 'Açık' },
    { value: 1, label: 'İşlemde' },
    { value: 2, label: 'Kullanıcı yanıtı bekleniyor' },
    { value: 3, label: 'Çözüldü' },
    { value: 4, label: 'Kapalı' }
  ];

  constructor(
    private readonly supportRequestService: SupportRequestService,
    private readonly departmentService: DepartmentService,
    private readonly toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadRequests();
    this.departmentService.listDepartments().subscribe({
      next: (response) => {
        if (response.success && response.data) this.departments = response.data;
        else this.toastr.error(response.message || 'Departmanlar yüklenemedi.');
      },
      error: (error) => this.toastr.error(this.errorMessage(error, 'Departmanlar yüklenemedi.'))
    });
  }

  get openRequestCount(): number {
    return this.requests.filter((request) => request.status === 0 || request.status === 1).length;
  }

  get filteredRequests(): SupportRequestResponse[] {
    const term = this.requestSearch.trim().toLocaleLowerCase('tr');
    if (!term) return this.requests;
    return this.requests.filter((request) =>
      [request.requestCode, request.fullname, request.email, request.title]
        .some((value) => value.toLocaleLowerCase('tr').includes(term))
    );
  }

  loadRequests(): void {
    this.loadingRequests = true;
    this.supportRequestService.listForAdmin().subscribe({
      next: (response) => {
        this.loadingRequests = false;
        if (response.success && response.data) {
          this.requests = response.data;
        }

        else {
          this.toastr.error(response.message || 'Destek talepleri yüklenemedi.');
        }
      },
      error: (error) => {
        this.loadingRequests = false;
        this.toastr.error(this.errorMessage(error, 'Destek talepleri yüklenemedi.'));
      }
    });
  }

  changeStatus(request: SupportRequestResponse, event: Event): void {
    const status = Number((event.target as HTMLSelectElement).value);
    this.supportRequestService.updateStatus(request.id, status).subscribe({
      next: (response) => {
        if (!response.success || !response.data) {
          this.toastr.error(response.message || 'Talep durumu güncellenemedi.');
          this.loadRequests();
          return;
        }
        this.replaceRequest(response.data);
        this.toastr.success('Talep durumu güncellendi.');
      },
      error: (error) => {
        this.toastr.error(this.errorMessage(error, 'Talep durumu güncellenemedi.'));
        this.loadRequests();
      }
    });
  }

  assignDepartment(request: SupportRequestResponse, event: Event): void {
    const departmentId = (event.target as HTMLSelectElement).value;
    if (!departmentId) return;
    this.supportRequestService.assignDepartment(request.id, departmentId).subscribe({
      next: (response) => {
        if (!response.success || !response.data) {
          this.toastr.error(response.message || 'Departman ataması yapılamadı.');
          this.loadRequests();
          return;
        }
        this.replaceRequest(response.data);
        this.toastr.success('Talep departmana atandı.');
      },
      error: (error) => {
        this.toastr.error(this.errorMessage(error, 'Departman ataması yapılamadı.'));
        this.loadRequests();
      }
    });
  }

  priorityLabel(priority: number): string {
    return ['Düşük', 'Normal', 'Yüksek'][priority - 1] || 'Normal';
  }

  statusClass(status: number): string {
    return `status-${status}`;
  }
  updateUserMessageInput($ev: Event) {
    let temp = this.showDetails;
    var checkbox = ($ev.target as HTMLInputElement)
    this.showTextarea = false;
        setTimeout(()=> this.showTextarea = true, 5);
    if (checkbox.checked) {
      this.updateUserMessage = true;
    } else {
      this.updateUserMessage = false;
    }
  }
  deleteSupportRequest(supportRequest: SupportRequestResponse) {
    if(window.confirm("Destek bileti silinecek onaylıyor musunuz")) {
    this.supportRequestService.deleteSupportRequest(supportRequest.id).subscribe({
      next: (response) => {
        this.toastr.success(response.message);
        this.loadRequests();
      }, error: (err) => {
        this.toastr.error(err.error?.message || err.error?.Message || "Bir hata oluştu");
      }
    })
    }

  }
  showDescription(request: SupportRequestResponse): void {
    //this.toastr.info(request.description, request.requestCode);
    this.showDetails = request.id;
    this.updateUserMessage = false;
    setTimeout(() => (document.getElementById("update-user-message") as HTMLInputElement).checked = false, 200);
  }
  updateMessage(request: SupportRequestResponse) {
    let messageTextarea = document.getElementById("update-message-textarea") as HTMLTextAreaElement;
    if (messageTextarea != null) {
      let updateUserMessageInput = document.getElementById("update-user-message") as HTMLInputElement;
      this.supportRequestService.updateMessage(request.id, messageTextarea.value, updateUserMessageInput.checked).subscribe({
        next: (response) => {
          this.toastr.success(updateUserMessageInput.checked ? "Kullanıcı mesajı başarıyla güncellendi" : "Destek talebi mesajı güncellendi");
        }, error: (err) => {
          this.toastr.error(err.error?.message ?? "Bir hata oluştu");
        }
      });
    }
  }

  private replaceRequest(updated: SupportRequestResponse): void {
    this.requests = this.requests.map((request) => request.id === updated.id ? updated : request);
  }

  private errorMessage(error: any, fallback: string): string {
    return error?.error?.message || error?.error?.Message || fallback;
  }
}