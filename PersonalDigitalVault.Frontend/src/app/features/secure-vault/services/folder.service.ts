import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';

import {
  Folder,
  CreateFolderRequest,
  UpdateFolderRequest
} from '../models/folder.model';

@Injectable({
  providedIn: 'root'
})
export class FolderService {

  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/folders`;

  // GET /api/folders
  getFolders(): Observable<Folder[]> {
    return this.http.get<Folder[]>(this.apiUrl);
  }

  // POST /api/folders
  createFolder(
    request: CreateFolderRequest
  ): Observable<Folder> {
    return this.http.post<Folder>(
      this.apiUrl,
      request
    );
  }

  // PUT /api/folders/{id}
  updateFolder(
    id: number,
    request: UpdateFolderRequest
  ): Observable<Folder> {
    return this.http.put<Folder>(
      `${this.apiUrl}/${id}`,
      request
    );
  }

  // DELETE /api/folders/{id}
  deleteFolder(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}