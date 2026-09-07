import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { DocumentItem } from '../models/document.model';
import { CredentialItem } from '../models/credential.model';
import { Folder } from '../models/folder.model';

export interface VaultSearchResults {
  folders: Folder[];
  documents: DocumentItem[];
  credentials: CredentialItem[];
}

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/search`;

  // Endpoint: GET /api/search?q=query
  searchVault(query: string): Observable<ApiResponse<VaultSearchResults>> {
    return this.http.get<ApiResponse<VaultSearchResults>>(`${this.apiUrl}?q=${encodeURIComponent(query)}`);
  }
}
