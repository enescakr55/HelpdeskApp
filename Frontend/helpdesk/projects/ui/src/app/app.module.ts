import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { GetSupportComponent } from './components/get-support/get-support.component';
import { RegisterComponent } from './components/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminLayoutComponent } from './components/admin-panel/layout/admin-layout.component';
import { SupportRequestsComponent } from './components/admin-panel/support-requests/support-requests.component';
import { AdminDepartmentsComponent } from './components/admin-panel/departments/admin-departments.component';
import { AdminUsersComponent } from './components/admin-panel/users/admin-users.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminLoginComponent,
    GetSupportComponent,
    RegisterComponent,
    AdminLayoutComponent,
    SupportRequestsComponent,
    AdminDepartmentsComponent,
    AdminUsersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({positionClass:"toast-bottom-right",closeButton:true,progressAnimation:"decreasing",progressBar:true,easing:"ease-in",easeTime:300,tapToDismiss:false})
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
