import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataResult } from '../models/responses/data-result';
import { DepartmentResponseModel } from '../models/responses/departmentResponseModel';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private httpClient:HttpClient) { }

  listDepartments(){
    return this.httpClient.get<DataResult<DepartmentResponseModel[]>>(`${environment.apiUrl}api/departments`)
  }

}
