import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DataResult } from '../models/responses/data-result';
import { CreateSupportRequest } from '../models/requests/createSupportRequest';
import { SupportRequestResponse } from '../models/requests/supportRequestResponse';
import { SupportRequestTrackResponseModel } from '../models/responses/supportRequestTrackResponseModel';

@Injectable({
  providedIn: 'root'
})
export class SupportRequestService {

  constructor(private readonly http: HttpClient) {}

  create(request: CreateSupportRequest): Observable<DataResult<SupportRequestResponse>> {
    return this.http.post<DataResult<SupportRequestResponse>>(`${environment.apiUrl}api/support-requests/create`, request);
  }

  getByRequestCode(requestCode: string): Observable<DataResult<SupportRequestTrackResponseModel>> {
    return this.http.get<DataResult<SupportRequestTrackResponseModel>>(
      `${environment.apiUrl}api/support-requests/code/${encodeURIComponent(requestCode)}`
    );
  }
  
  listForAdmin(): Observable<DataResult<SupportRequestResponse[]>> {
    return this.http.get<DataResult<SupportRequestResponse[]>>(`${environment.apiUrl}api/support-requests`);
  }
  
  updateStatus(id: string, status: number): Observable<DataResult<SupportRequestResponse>> {
    return this.http.post<DataResult<SupportRequestResponse>>(
      `${environment.apiUrl}api/support-requests/${id}/status`,
      { status }
    );
  }
  
  assignDepartment(id: string, departmentId: string): Observable<DataResult<SupportRequestResponse>> {
    return this.http.post<DataResult<SupportRequestResponse>>(
      `${environment.apiUrl}api/support-requests/${id}/assign-department/${departmentId}`,
      {}
    );
  }
  updateMessage(id:string,message:string,updateUserMessage:boolean){
    return this.http.post<DataResult<SupportRequestResponse>>(`${environment.apiUrl}api/support-requests/${id}/message`,{message:message,isUserMessage:updateUserMessage});
  }
}
