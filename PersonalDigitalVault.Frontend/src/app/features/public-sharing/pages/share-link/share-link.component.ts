import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PublicSharingService } from '../../services/public-sharing.service';
import { DocumentService } from '../../../secure-vault/services/document.service';
import { ShareLink } from '../../models/share-link.model';
import { DocumentItem } from '../../../secure-vault/models/document.model';
import { ShareLinkCardComponent } from '../../components/share-link-card/share-link-card.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-share-link',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ShareLinkCardComponent,
    LoadingComponent,
    EmptyStateComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './share-link.component.html',
  styleUrl: './share-link.component.css'
})
export class ShareLinkComponent implements OnInit {
  private fb = inject(FormBuilder);
  private sharingService = inject(PublicSharingService);
  private documentService = inject(DocumentService);

  links: ShareLink[] = [];
  documents: DocumentItem[] = [];
  isLoading = true;
  showCreateModal = false;

  showRevokeDialog = false;
  selectedLinkId: string | null = null;

  createForm: FormGroup = this.fb.group({
    documentId: ['', [Validators.required]],
    expiryDays: [7, [Validators.required, Validators.min(1)]]
  });

  ngOnInit(): void {
    this.loadLinks();
    this.loadDocuments();
  }

  loadLinks(): void {
    this.isLoading = true;
    this.sharingService.getShareLinks().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.links = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
        this.links = [];
      }
    });
  }

  loadDocuments(): void {
    this.documentService.getDocuments().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.documents = res.data;
        }
      }
    });
  }

  onCreateShareLink(): void {
    if (this.createForm.invalid) return;

    this.sharingService.createShareLink(this.createForm.value).subscribe({
      next: (res) => {
        if (res.success) {
          this.showCreateModal = false;
          this.createForm.reset({ expiryDays: 7 });
          this.loadLinks();
        }
      }
    });
  }

  onPromptRevoke(id: string): void {
    this.selectedLinkId = id;
    this.showRevokeDialog = true;
  }

  confirmRevoke(): void {
    if (!this.selectedLinkId) return;
    this.sharingService.revokeShareLink(this.selectedLinkId).subscribe({
      next: () => {
        this.showRevokeDialog = false;
        this.loadLinks();
      },
      error: () => {
        this.showRevokeDialog = false;
      }
    });
  }
}
