import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataResult } from '../models/responses/data-result';
import { DepartmentResponseModel } from '../models/responses/departmentResponseModel';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private httpClient:HttpClient) { }


  listDepartments(): Observable<DataResult<DepartmentResponseModel[]>> {
    return this.httpClient.get<DataResult<DepartmentResponseModel[]>>(`${environment.apiUrl}api/departments`);
  }
  
  createDepartment(departmentName: string): Observable<DataResult<DepartmentResponseModel>> {
    return this.httpClient.post<DataResult<DepartmentResponseModel>>(`${environment.apiUrl}api/departments`, { departmentName });
  }
  
  updateDepartment(id: string, departmentName: string): Observable<DataResult<DepartmentResponseModel>> {
    return this.httpClient.post<DataResult<DepartmentResponseModel>>(
      `${environment.apiUrl}api/departments/update/${id}`,
      { departmentName }
    );
  }
  
  deleteDepartment(id: string): Observable<DataResult<boolean>> {
    return this.httpClient.delete<DataResult<boolean>>(`${environment.apiUrl}api/departments/${id}`);
  }

}
