import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  ShareLink,
  CreateShareLinkRequest,
  PublicFileDetails
} from '../models/share-link.model';

@Injectable({
  providedIn: 'root'
})
export class PublicSharingService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}`;

  // GET /api/share-links
  getShareLinks(): Observable<ShareLink[]> {
    return this.http.get<ShareLink[]>(
      `${this.apiUrl}/share-links`
    );
  }

  // POST /api/share-links
  createShareLink(
    request: CreateShareLinkRequest
  ): Observable<ShareLink> {
    return this.http.post<ShareLink>(
      `${this.apiUrl}/share-links`,
      request
    );
  }

  // PUT /api/share-links/{id}
  updateShareLinkExpiry(
    id: number,
    expiresAt: string | null
  ): Observable<ShareLink> {
    return this.http.put<ShareLink>(
      `${this.apiUrl}/share-links/${id}`,
      { expiresAt }
    );
  }

  // POST /api/share-links/{id}/revoke
  revokeShareLink(id: number): Observable<ShareLink> {
    return this.http.post<ShareLink>(
      `${this.apiUrl}/share-links/${id}/revoke`,
      {}
    );
  }

  // DELETE /api/share-links/{id}
deleteShareLink(id: number): Observable<void> {
  return this.http.delete<void>(
    `${this.apiUrl}/share-links/${id}`
  );
}

  // GET /api/public/share/{token}
  getPublicFileDetails(
    token: string
  ): Observable<PublicFileDetails> {
    return this.http.get<PublicFileDetails>(
      `${this.apiUrl}/public/share/${token}`
    );
  }

  // GET /api/public/share/{token}/download
  downloadPublicFile(token: string): Observable<Blob> {
    return this.http.get(
      `${this.apiUrl}/public/share/${token}/download`,
      {
        responseType: 'blob'
      }
    );
  }
}