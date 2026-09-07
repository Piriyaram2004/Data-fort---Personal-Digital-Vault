import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { DocumentItem } from '../models/document.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/documents`;

  // Endpoint: GET /api/documents
  getDocuments(folderId?: string): Observable<ApiResponse<DocumentItem[]>> {
    const url = folderId ? `${this.apiUrl}?folderId=${folderId}` : this.apiUrl;
    return this.http.get<ApiResponse<DocumentItem[]>>(url);
  }

  // Endpoint: GET /api/documents/:id
  getDocumentById(id: string): Observable<ApiResponse<DocumentItem>> {
    return this.http.get<ApiResponse<DocumentItem>>(`${this.apiUrl}/${id}`);
  }

  // Endpoint: POST /api/documents/upload
  uploadDocument(formData: FormData): Observable<ApiResponse<DocumentItem>> {
    return this.http.post<ApiResponse<DocumentItem>>(`${this.apiUrl}/upload`, formData);
  }

  // Endpoint: GET /api/documents/:id/download
  downloadDocument(id: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${id}/download`, { responseType: 'blob' });
  }

  // Endpoint: DELETE /api/documents/:id
  deleteDocument(id: string): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
