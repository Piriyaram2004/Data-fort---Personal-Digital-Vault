import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { PublicSharingService } from '../../services/public-sharing.service';
import { DocumentService } from '../../../secure-vault/services/document.service';

import {
  ShareLink,
  CreateShareLinkRequest
} from '../../models/share-link.model';

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
  private readonly fb = inject(FormBuilder);
  private readonly sharingService = inject(PublicSharingService);
  private readonly documentService = inject(DocumentService);

  links: ShareLink[] = [];
  documents: DocumentItem[] = [];

  isLoading = true;
  showCreateModal = false;

  // Revoke dialog
  showRevokeDialog = false;
  selectedLinkId: number | null = null;

  // Delete dialog
  showDeleteDialog = false;
  selectedDeleteLinkId: number | null = null;

  createForm: FormGroup = this.fb.group({
    documentId: ['', [Validators.required]],
    expiresAt: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.loadLinks();
    this.loadDocuments();
  }

  /**
   * Load all share links belonging to the logged-in user.
   */
  loadLinks(): void {
    this.isLoading = true;

    this.sharingService.getShareLinks().subscribe({
      next: (links) => {
        this.links = links;
        this.isLoading = false;
      },
      error: () => {
        this.links = [];
        this.isLoading = false;
      }
    });
  }

  /**
   * Load documents that can be shared.
   */
  loadDocuments(): void {
    this.documentService.getDocuments().subscribe({
      next: (documents: DocumentItem[]) => {
        this.documents = documents;
      },
      error: (error) => {
        console.error('Failed to load documents:', error);
        this.documents = [];
      }
    });
  }

  /**
   * Create a new public share link.
   */
  onCreateShareLink(): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    const request: CreateShareLinkRequest = {
      documentId: Number(this.createForm.value.documentId),
      expiresAt: this.createForm.value.expiresAt
    };

    this.sharingService.createShareLink(request).subscribe({
      next: () => {
        this.showCreateModal = false;

        this.createForm.reset({
          documentId: '',
          expiresAt: ''
        });

        this.loadLinks();
      },
      error: (error) => {
        console.error('Failed to create share link:', error);
      }
    });
  }

  /**
   * Open the revoke confirmation dialog.
   */
  onPromptRevoke(id: number): void {
    this.selectedLinkId = id;
    this.showRevokeDialog = true;
  }

  /**
   * Revoke the selected share link.
   */
  confirmRevoke(): void {
    if (this.selectedLinkId === null) {
      return;
    }

    this.sharingService
      .revokeShareLink(this.selectedLinkId)
      .subscribe({
        next: () => {
          this.showRevokeDialog = false;
          this.selectedLinkId = null;

          this.loadLinks();
        },
        error: (error) => {
          console.error('Failed to revoke share link:', error);

          this.showRevokeDialog = false;
          this.selectedLinkId = null;
        }
      });
  }

  /**
   * Open the permanent delete confirmation dialog.
   */
  onPromptDelete(id: number): void {
    this.selectedDeleteLinkId = id;
    this.showDeleteDialog = true;
  }

  /**
   * Permanently delete the selected share link.
   *
   * Once deleted, the ShareLink database record no longer exists.
   * Therefore the old public URL becomes invalid immediately.
   */
  confirmDelete(): void {
    if (this.selectedDeleteLinkId === null) {
      return;
    }

    this.sharingService
      .deleteShareLink(this.selectedDeleteLinkId)
      .subscribe({
        next: () => {
          this.showDeleteDialog = false;
          this.selectedDeleteLinkId = null;

          // Refresh the list after successful deletion.
          this.loadLinks();
        },
        error: (error) => {
          console.error('Failed to delete share link:', error);

          this.showDeleteDialog = false;
          this.selectedDeleteLinkId = null;
        }
      });
  }
}