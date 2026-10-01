import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DepartmentResponseModel } from '../../../models/responses/departmentResponseModel';
import { DepartmentService } from '../../../services/department.service';

@Component({
  selector: 'app-admin-departments',
  standalone: false,
  templateUrl: './admin-departments.component.html',
  styleUrl: './admin-departments.component.scss'
})
export class AdminDepartmentsComponent implements OnInit {
  departments: DepartmentResponseModel[] = [];
  loadingDepartments = false;
  savingDepartment = false;
  showEditor = false;
  editingDepartmentId: string | null = null;
  departmentSearch = '';
  readonly departmentForm: FormGroup;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly departmentService: DepartmentService,
    private readonly toastr: ToastrService
  ) {
    this.departmentForm = this.formBuilder.group({
      departmentName: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  ngOnInit(): void {
    this.loadDepartments();
  }

  get filteredDepartments(): DepartmentResponseModel[] {
    const term = this.departmentSearch.trim().toLocaleLowerCase('tr');
    return this.departments.filter((department) => department.departmentName.toLocaleLowerCase('tr').includes(term));
  }

  loadDepartments(): void {
    this.loadingDepartments = true;
    this.departmentService.listDepartments().subscribe({
      next: (response) => {
        this.loadingDepartments = false;
        if (response.success && response.data) this.departments = response.data;
        else this.toastr.error(response.message || 'Departmanlar yüklenemedi.');
      },
      error: (error) => {
        this.loadingDepartments = false;
        this.toastr.error(this.errorMessage(error, 'Departmanlar yüklenemedi.'));
      }
    });
  }

  openNewDepartment(): void {
    this.editingDepartmentId = null;
    this.departmentForm.reset({ departmentName: '' });
    this.showEditor = true;
  }

  editDepartment(department: DepartmentResponseModel): void {
    this.editingDepartmentId = department.id;
    this.departmentForm.reset({ departmentName: department.departmentName });
    this.showEditor = true;
  }

  closeEditor(): void {
    this.showEditor = false;
    this.editingDepartmentId = null;
    this.departmentForm.reset({ departmentName: '' });
  }

  saveDepartment(): void {
    this.departmentForm.markAllAsTouched();
    if (this.departmentForm.invalid) return;
    const name = String(this.departmentForm.controls['departmentName'].value).trim();
    this.savingDepartment = true;
    const request = this.editingDepartmentId
      ? this.departmentService.updateDepartment(this.editingDepartmentId, name)
      : this.departmentService.createDepartment(name);

    request.subscribe({
      next: (response) => {
        this.savingDepartment = false;
        if (!response.success) {
          this.toastr.error(response.message || 'Departman kaydedilemedi.');
          return;
        }
        this.toastr.success(response.message || 'Departman kaydedildi.');
        this.closeEditor();
        this.loadDepartments();
      },
      error: (error) => {
        this.savingDepartment = false;
        this.toastr.error(this.errorMessage(error, 'Departman kaydedilemedi.'));
      }
    });
  }

  deleteDepartment(department: DepartmentResponseModel): void {
    if (!window.confirm(`“${department.departmentName}” departmanı silinsin mi?`)) return;
    this.departmentService.deleteDepartment(department.id).subscribe({
      next: (response) => {
        if (!response.success) {
          this.toastr.error(response.message || 'Departman silinemedi.');
          return;
        }
        this.toastr.success(response.message || 'Departman silindi.');
        this.loadDepartments();
      },
      error: (error) => this.toastr.error(this.errorMessage(error, 'Departman silinemedi.'))
    });
  }

  private errorMessage(error: any, fallback: string): string {
    return error?.error?.message || error?.error?.Message || fallback;
  }
}