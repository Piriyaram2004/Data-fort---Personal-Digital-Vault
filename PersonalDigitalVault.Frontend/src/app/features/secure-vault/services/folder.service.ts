import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { Folder, CreateFolderRequest } from '../models/folder.model';

@Injectable({
  providedIn: 'root'
})
export class FolderService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/folders`;

  // Endpoint: GET /api/folders
  getFolders(): Observable<ApiResponse<Folder[]>> {
    return this.http.get<ApiResponse<Folder[]>>(this.apiUrl);
  }

  // Endpoint: GET /api/folders/:id
  getFolderById(id: string): Observable<ApiResponse<Folder>> {
    return this.http.get<ApiResponse<Folder>>(`${this.apiUrl}/${id}`);
  }

  // Endpoint: POST /api/folders
  createFolder(folder: CreateFolderRequest): Observable<ApiResponse<Folder>> {
    return this.http.post<ApiResponse<Folder>>(this.apiUrl, folder);
  }

  // Endpoint: PUT /api/folders/:id
  updateFolder(id: string, folder: CreateFolderRequest): Observable<ApiResponse<Folder>> {
    return this.http.put<ApiResponse<Folder>>(`${this.apiUrl}/${id}`, folder);
  }

  // Endpoint: DELETE /api/folders/:id
  deleteFolder(id: string): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
