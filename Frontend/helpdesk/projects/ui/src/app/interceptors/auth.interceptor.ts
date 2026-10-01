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
    const token = localStorage.getItem('authToken');

    if (!token || request.headers.has('Authorization')) {
      return next.handle(request);
    }

    const authenticatedRequest = request.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });

    return next.handle(authenticatedRequest);
  }
}
