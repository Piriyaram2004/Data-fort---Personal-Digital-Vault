import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface SearchFolderResult {
  folderId: number;
  folderName: string;
  description: string | null;
}

export interface SearchDocumentResult {
  documentId: number;
  folderId: number | null;
  originalFileName: string;
  fileType: string;
}

export interface SearchCredentialResult {
  credentialId: number;
  folderId: number | null;
  title: string;
  userName: string;
  notes: string | null;
}

export interface VaultSearchResults {
  folders: SearchFolderResult[];
  documents: SearchDocumentResult[];
  credentials: SearchCredentialResult[];
}

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/search`;

  // Endpoint: GET /api/search?searchTerm=query
  searchVault(searchTerm: string): Observable<VaultSearchResults> {
    return this.http.get<VaultSearchResults>(
      `${this.apiUrl}?searchTerm=${encodeURIComponent(searchTerm)}`
    );
  }
}