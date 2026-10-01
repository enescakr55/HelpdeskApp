import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { GetSupportComponent } from './components/get-support/get-support.component';
import { RegisterComponent } from './components/register/register.component';

const routes: Routes = [
  { path: '', component: GetSupportComponent },
  { path: 'admin/login', component: AdminLoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
