import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { ShareLink, CreateShareLinkRequest, PublicFileDetails } from '../models/share-link.model';

@Injectable({
  providedIn: 'root'
})
export class PublicSharingService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/public-sharing`;

  // Endpoint: GET /api/public-sharing/links
  getShareLinks(): Observable<ApiResponse<ShareLink[]>> {
    return this.http.get<ApiResponse<ShareLink[]>>(`${this.apiUrl}/links`);
  }

  // Endpoint: POST /api/public-sharing/links
  createShareLink(request: CreateShareLinkRequest): Observable<ApiResponse<ShareLink>> {
    return this.http.post<ApiResponse<ShareLink>>(`${this.apiUrl}/links`, request);
  }

  // Endpoint: POST /api/public-sharing/links/:id/revoke
  revokeShareLink(id: string): Observable<ApiResponse<void>> {
    return this.http.post<ApiResponse<void>>(`${this.apiUrl}/links/${id}/revoke`, {});
  }

  // Endpoint: GET /api/public-sharing/file/:token (Public unauthenticated access)
  getPublicFileDetails(token: string): Observable<ApiResponse<PublicFileDetails>> {
    return this.http.get<ApiResponse<PublicFileDetails>>(`${this.apiUrl}/file/${token}`);
  }

  // Endpoint: GET /api/public-sharing/file/:token/download (Public download)
  downloadPublicFile(token: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/file/${token}/download`, { responseType: 'blob' });
  }
}
