import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { GetSupportComponent } from './components/get-support/get-support.component';
import { RegisterComponent } from './components/register/register.component';
import { AdminLayoutComponent } from './components/admin-panel/layout/admin-layout.component';
import { AuthGuard } from './guards/auth.guard';
import { SupportRequestsComponent } from './components/admin-panel/support-requests/support-requests.component';
import { AdminDepartmentsComponent } from './components/admin-panel/departments/admin-departments.component';
import { AdminUsersComponent } from './components/admin-panel/users/admin-users.component';
import { LandingComponent } from './components/landing/landing.component';

const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'support/request', component: GetSupportComponent },
  { path: 'admin/login', component: AdminLoginComponent },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'support-requests' },
      { path: 'support-requests', component: SupportRequestsComponent },
      { path: 'departments', component: AdminDepartmentsComponent },
      { path: 'users', component: AdminUsersComponent }
    ]
  },
  { path: 'register', component: RegisterComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
