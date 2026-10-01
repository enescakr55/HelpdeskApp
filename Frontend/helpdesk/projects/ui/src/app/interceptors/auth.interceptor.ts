import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const isApiRequest = request.url.startsWith(environment.apiUrl);
    const isAuthRequest = request.url.startsWith(`${environment.apiUrl}api/auth/`);
    const token = localStorage.getItem('authToken');

    if (!isApiRequest || isAuthRequest || !token || request.headers.has('Authorization')) {
      return next.handle(request);
    }

    const authenticatedRequest = request.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });

    return next.handle(authenticatedRequest);
  }
}
