import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { LoginRequest } from '../models/login.model';
import {RegisterRequest, RegisterResponse} from '../models/register.model';
import { AuthResponse } from '../models/auth-response.model';
import { UserProfile, UpdateProfileRequest } from '../models/profile.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/auth`;

  // Endpoint: POST /api/auth/login
  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials);
  }

  // Endpoint: POST /api/auth/register
  register(data: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(
      `${this.apiUrl}/register`,
      data
    );
  }

  // Endpoint: POST /api/auth/forgot-password
forgotPassword(email: string): Observable<{ message: string }> {
  return this.http.post<{ message: string }>(
    `${this.apiUrl}/forgot-password`,
    { email }
  );
}

  // Endpoint: POST /api/auth/reset-password
resetPassword(
  email: string,
  token: string,
  newPassword: string,
  confirmPassword: string
): Observable<{ message: string }> {
  return this.http.post<{ message: string }>(
    `${this.apiUrl}/reset-password`,
    {
      email,
      token,
      newPassword,
      confirmPassword
    }
  );
}

  // Endpoint: POST /api/auth/change-password
changePassword(
  currentPassword: string,
  newPassword: string,
  confirmPassword: string
): Observable<{ message: string }> {
  return this.http.post<{ message: string }>(
    `${this.apiUrl}/change-password`,
    {
      currentPassword,
      newPassword,
      confirmPassword
    }
  );
}

  // Endpoint: GET /api/auth/profile
  getProfile(): Observable<ApiResponse<UserProfile>> {
    return this.http.get<ApiResponse<UserProfile>>(`${this.apiUrl}/profile`);
  }

  // Endpoint: PUT /api/auth/profile
  updateProfile(data: UpdateProfileRequest): Observable<ApiResponse<UserProfile>> {
    return this.http.put<ApiResponse<UserProfile>>(`${this.apiUrl}/profile`, data);
  }
}
