import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { CredentialItem, CreateCredentialRequest } from '../models/credential.model';

@Injectable({
  providedIn: 'root'
})
export class CredentialService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/credentials`;

  // Endpoint: GET /api/credentials
  getCredentials(): Observable<ApiResponse<CredentialItem[]>> {
    return this.http.get<ApiResponse<CredentialItem[]>>(this.apiUrl);
  }

  // Endpoint: GET /api/credentials/:id
  getCredentialById(id: string): Observable<ApiResponse<CredentialItem>> {
    return this.http.get<ApiResponse<CredentialItem>>(`${this.apiUrl}/${id}`);
  }

  // Endpoint: POST /api/credentials
  createCredential(credential: CreateCredentialRequest): Observable<ApiResponse<CredentialItem>> {
    return this.http.post<ApiResponse<CredentialItem>>(this.apiUrl, credential);
  }

  // Endpoint: PUT /api/credentials/:id
  updateCredential(id: string, credential: CreateCredentialRequest): Observable<ApiResponse<CredentialItem>> {
    return this.http.put<ApiResponse<CredentialItem>>(`${this.apiUrl}/${id}`, credential);
  }

  // Endpoint: DELETE /api/credentials/:id
  deleteCredential(id: string): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
