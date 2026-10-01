import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DataResult } from '../models/responses/data-result';
import { UserResponseModel } from '../models/responses/userResponseModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private readonly http: HttpClient) {}

  listUsers(): Observable<DataResult<UserResponseModel[]>> {
    return this.http.get<DataResult<UserResponseModel[]>>(`${environment.apiUrl}api/users`);
  }

  listPendingManagers(): Observable<DataResult<UserResponseModel[]>> {
    return this.http.get<DataResult<UserResponseModel[]>>(`${environment.apiUrl}api/users/pending-approvals`);
  }

  approveManager(userId: string): Observable<DataResult<UserResponseModel>> {
    return this.http.post<DataResult<UserResponseModel>>(
      `${environment.apiUrl}api/users/approve-manager`,
      { userId }
    );
  }
}