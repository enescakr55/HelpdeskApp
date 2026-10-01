import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  sidebarOpen = false;

  constructor(private readonly router: Router) {}

  closeSidebar(): void {
    this.sidebarOpen = false;
  }

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('authRole');
    this.router.navigate(['/admin/login']);
  }
}