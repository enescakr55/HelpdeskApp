import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private readonly router: Router) {}

  canActivate(): boolean {
    const isAdmin = localStorage.getItem('authRole') === 'Admin';
    const hasToken = Boolean(localStorage.getItem('authToken'));

    if (isAdmin && hasToken) return true;

    this.router.navigate(['/admin/login']);
    return false;
  }
}
