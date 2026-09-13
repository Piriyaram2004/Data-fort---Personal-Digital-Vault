import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';

import {
  CredentialItem,
  CreateCredentialRequest,
  UpdateCredentialRequest
} from '../models/credential.model';

@Injectable({
  providedIn: 'root'
})
export class CredentialService {
  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/credentials`;

  getCredentials(): Observable<CredentialItem[]> {
    return this.http.get<CredentialItem[]>(this.apiUrl);
  }

  createCredential(
    credential: CreateCredentialRequest
  ): Observable<CredentialItem> {
    return this.http.post<CredentialItem>(
      this.apiUrl,
      credential
    );
  }

  updateCredential(
    id: number,
    credential: UpdateCredentialRequest
  ): Observable<CredentialItem> {
    return this.http.put<CredentialItem>(
      `${this.apiUrl}/${id}`,
      credential
    );
  }

  deleteCredential(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}