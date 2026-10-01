import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DepartmentResponseModel } from '../../../models/responses/departmentResponseModel';
import { UserResponseModel } from '../../../models/responses/userResponseModel';
import { DepartmentService } from '../../../services/department.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-admin-users',
  standalone: false,
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.scss'
})
export class AdminUsersComponent implements OnInit {
  users: UserResponseModel[] = [];
  pendingManagers: UserResponseModel[] = [];
  departments: DepartmentResponseModel[] = [];
  loadingUsers = false;
  userSearch = '';

  constructor(
    private readonly userService: UserService,
    private readonly departmentService: DepartmentService,
    private readonly toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadPendingManagers();
    this.departmentService.listDepartments().subscribe({
      next: (response) => {
        if (response.success && response.data) this.departments = response.data;
        else this.toastr.error(response.message || 'Departmanlar yüklenemedi.');
      },
      error: (error) => this.toastr.error(this.errorMessage(error, 'Departmanlar yüklenemedi.'))
    });
  }

  get filteredUsers(): UserResponseModel[] {
    const term = this.userSearch.trim().toLocaleLowerCase('tr');
    if (!term) return this.users;
    return this.users.filter((user) =>
      [user.firstname, user.lastname, user.email]
        .some((value) => value.toLocaleLowerCase('tr').includes(term))
    );
  }

  loadUsers(): void {
    this.loadingUsers = true;
    this.userService.listUsers().subscribe({
      next: (response) => {
        this.loadingUsers = false;
        if (response.success && response.data) this.users = response.data;
        else this.toastr.error(response.message || 'Kullanıcılar yüklenemedi.');
      },
      error: (error) => {
        this.loadingUsers = false;
        this.toastr.error(this.errorMessage(error, 'Kullanıcılar yüklenemedi.'));
      }
    });
  }

  loadPendingManagers(): void {
    this.userService.listPendingManagers().subscribe({
      next: (response) => {
        if (response.success && response.data) this.pendingManagers = response.data;
        else this.toastr.error(response.message || 'Bekleyen başvurular yüklenemedi.');
      },
      error: (error) => this.toastr.error(this.errorMessage(error, 'Bekleyen başvurular yüklenemedi.'))
    });
  }

  approveManager(user: UserResponseModel): void {
    this.userService.approveManager(user.id).subscribe({
      next: (response) => {
        if (!response.success) {
          this.toastr.error(response.message || 'Yönetici başvurusu onaylanamadı.');
          return;
        }
        this.toastr.success(`${user.firstname} ${user.lastname} yönetici olarak onaylandı.`);
        this.loadUsers();
        this.loadPendingManagers();
      },
      error: (error) => this.toastr.error(this.errorMessage(error, 'Yönetici başvurusu onaylanamadı.'))
    });
  }

  departmentName(id: string): string {
    return this.departments.find((department) => department.id === id)?.departmentName || 'Atanmadı';
  }

  private errorMessage(error: any, fallback: string): string {
    return error?.error?.message || error?.error?.Message || fallback;
  }
}