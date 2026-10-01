import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { AuthResponseModel } from '../models/responses/authResponseModel';
import { DataResult } from '../models/responses/data-result';
import { LoginRequest } from '../models/requests/loginRequest';
import { RegisterRequest } from '../models/requests/registerRequest';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private readonly httpClient:HttpClient) { }
  login(req:LoginRequest){
    return this.httpClient.post<DataResult<AuthResponseModel>>(`${environment.apiUrl}api/auth/login`,req);
  }
  register(req:RegisterRequest){
    return this.httpClient.post<DataResult<AuthResponseModel>>(`${environment.apiUrl}api/auth/register`,req);
  }
}
