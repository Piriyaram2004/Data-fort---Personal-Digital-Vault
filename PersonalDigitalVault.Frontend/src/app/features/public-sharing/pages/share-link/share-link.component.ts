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

  showRevokeDialog = false;
  selectedLinkId: number | null = null;

  createForm: FormGroup = this.fb.group({
    documentId: ['', [Validators.required]],
    expiresAt: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.loadLinks();
    this.loadDocuments();
  }

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
      }
    });
  }

  onPromptRevoke(id: number): void {
    this.selectedLinkId = id;
    this.showRevokeDialog = true;
  }

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
        error: () => {
          this.showRevokeDialog = false;
          this.selectedLinkId = null;
        }
      });
  }
}