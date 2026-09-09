import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { DocumentItem } from '../models/document.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/documents`;

  // API #5
  // GET /api/documents
  getDocuments(): Observable<DocumentItem[]> {
    return this.http.get<DocumentItem[]>(this.apiUrl);
  }

  // API #9
  // POST /api/documents/upload
  uploadDocument(formData: FormData): Observable<DocumentItem> {
    return this.http.post<DocumentItem>(
      `${this.apiUrl}/upload`,
      formData
    );
  }

  // API #10
  // GET /api/documents/{id}/download
  downloadDocument(id: number): Observable<Blob> {
    return this.http.get(
      `${this.apiUrl}/${id}/download`,
      {
        responseType: 'blob'
      }
    );
  }

  // API #11
  // POST /api/documents/{id}/verify-integrity
  verifyIntegrity(
    id: number
  ): Observable<{
    documentId: number;
    isIntegrityValid: boolean;
  }> {
    return this.http.post<{
      documentId: number;
      isIntegrityValid: boolean;
    }>(
      `${this.apiUrl}/${id}/verify-integrity`,
      {}
    );
  }

  // API #8
  // DELETE /api/documents/{id}
  deleteDocument(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}