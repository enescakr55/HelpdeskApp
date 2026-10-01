import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DataResult } from '../models/responses/data-result';
import { CreateSupportRequest } from '../models/requests/createSupportRequest';
import { SupportRequestResponse } from '../models/requests/supportRequestResponse';

@Injectable({
  providedIn: 'root'
})
export class SupportRequestService {

  constructor(private readonly http: HttpClient) {}

  create(request: CreateSupportRequest): Observable<DataResult<SupportRequestResponse>> {
    return this.http.post<DataResult<SupportRequestResponse>>(`${environment.apiUrl}api/support-requests/create`, request);
  }
}
