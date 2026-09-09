import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

import { CredentialService } from '../../../services/credential.service';
import { CredentialItem } from '../../../models/credential.model';

import { CredentialCardComponent } from '../../../components/credential-card/credential-card.component';
import { EmptyStateComponent } from '../../../../../shared/components/empty-state/empty-state.component';
import { ConfirmDialogComponent } from '../../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-credential-list',
  standalone: true,
  imports: [
    RouterLink,
    CredentialCardComponent,
    EmptyStateComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './credential-list.component.html',
  styleUrl: './credential-list.component.css'
})
export class CredentialListComponent implements OnInit {
  private credentialService = inject(CredentialService);
  private router = inject(Router);

  credentials: CredentialItem[] = [];

  isLoading = true;
  errorMessage = '';

  showDeleteDialog = false;
  selectedCredentialId: number | null = null;

  ngOnInit(): void {
    this.loadCredentials();
  }

  loadCredentials(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.credentialService.getCredentials().subscribe({
      next: (credentials: CredentialItem[]) => {
        this.credentials = credentials;
        this.isLoading = false;
      },

      error: (error) => {
        console.error('Failed to load credentials:', error);

        this.credentials = [];
        this.isLoading = false;
        this.errorMessage = 'Unable to load credentials.';
      }
    });
  }

  onEditCredential(credential: CredentialItem): void {
    this.router.navigate([
      '/vault/credentials/edit',
      credential.credentialId
    ]);
  }

  onPromptDeleteCredential(id: number): void {
    this.selectedCredentialId = id;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {
    if (this.selectedCredentialId === null) {
      return;
    }

    const credentialId = this.selectedCredentialId;

    this.credentialService.deleteCredential(credentialId).subscribe({
      next: () => {
        this.showDeleteDialog = false;
        this.selectedCredentialId = null;
        this.loadCredentials();
      },

      error: (error) => {
        console.error('Failed to delete credential:', error);

        this.showDeleteDialog = false;
        this.selectedCredentialId = null;
        this.errorMessage = 'Unable to delete credential.';
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteDialog = false;
    this.selectedCredentialId = null;
  }
}