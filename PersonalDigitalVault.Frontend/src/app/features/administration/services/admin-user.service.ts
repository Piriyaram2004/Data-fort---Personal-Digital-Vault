import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  AdminUser,
  UpdateAdminUserStatusRequest
} from '../models/admin-user.model';

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/admin/users`;

  getUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(this.apiUrl);
  }

  updateUserStatus(
    userId: number,
    request: UpdateAdminUserStatusRequest
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${userId}/status`,
      request
    );
  }
}