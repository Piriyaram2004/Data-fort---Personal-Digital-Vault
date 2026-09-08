import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { AdminUser, UpdateUserRoleRequest } from '../models/admin-user.model';

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/admin/users`;

  // Endpoint: GET /api/admin/users
  getUsers(): Observable<ApiResponse<AdminUser[]>> {
    return this.http.get<ApiResponse<AdminUser[]>>(this.apiUrl);
  }

  // Endpoint: PUT /api/admin/users/:id
  updateUser(id: string, data: UpdateUserRoleRequest): Observable<ApiResponse<AdminUser>> {
    return this.http.put<ApiResponse<AdminUser>>(`${this.apiUrl}/${id}`, data);
  }

  // Endpoint: DELETE /api/admin/users/:id
  deleteUser(id: string): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
