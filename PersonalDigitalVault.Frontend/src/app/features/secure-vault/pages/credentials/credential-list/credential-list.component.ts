import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CredentialService } from '../../../services/credential.service';
import { CredentialItem } from '../../../models/credential.model';
import { CredentialCardComponent } from '../../../components/credential-card/credential-card.component';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../../shared/components/empty-state/empty-state.component';
import { ConfirmDialogComponent } from '../../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-credential-list',
  standalone: true,
  imports: [
    RouterLink,
    CredentialCardComponent,
    LoadingComponent,
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

  showDeleteDialog = false;
  selectedCredentialId: string | null = null;

  ngOnInit(): void {
    this.loadCredentials();
  }

  loadCredentials(): void {
    this.isLoading = true;
    this.credentialService.getCredentials().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.credentials = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
        this.credentials = [];
      }
    });
  }

  onEditCredential(credential: CredentialItem): void {
    this.router.navigate(['/vault/credentials/edit', credential.id]);
  }

  onPromptDeleteCredential(id: string): void {
    this.selectedCredentialId = id;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {
    if (!this.selectedCredentialId) return;
    this.credentialService.deleteCredential(this.selectedCredentialId).subscribe({
      next: () => {
        this.showDeleteDialog = false;
        this.loadCredentials();
      },
      error: () => {
        this.showDeleteDialog = false;
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteDialog = false;
    this.selectedCredentialId = null;
  }
}
