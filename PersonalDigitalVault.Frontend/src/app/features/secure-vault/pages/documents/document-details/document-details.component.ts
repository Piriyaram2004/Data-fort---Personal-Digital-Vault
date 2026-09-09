import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { finalize } from 'rxjs';

import { DocumentService } from '../../../services/document.service';
import { DocumentItem } from '../../../models/document.model';

import { FileSizePipe } from '../../../../../shared/pipes/file-size.pipe';
import { IntegrityBadgeComponent } from '../../../components/integrity-badge/integrity-badge.component';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { ConfirmDialogComponent } from '../../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-document-details',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe,
    FileSizePipe,
    IntegrityBadgeComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './document-details.component.html',
  styleUrl: './document-details.component.css'
})
export class DocumentDetailsComponent implements OnInit {

  private documentService = inject(DocumentService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  document: DocumentItem | null = null;

  isLoading = true;
  errorMessage = '';

  showDeleteDialog = false;

  integrityVerified: boolean | null = null;
  integrityMessage = '';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    console.log('Document Details - Route ID:', id);

    if (!id) {
      this.isLoading = false;
      this.errorMessage = 'Document ID is missing.';
      return;
    }

    const documentId = Number(id);

    if (!Number.isInteger(documentId) || documentId <= 0) {
      this.isLoading = false;
      this.errorMessage = 'Invalid document ID.';
      return;
    }

    this.loadDocumentDetails(documentId);
  }

  loadDocumentDetails(documentId: number): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.document = null;

    console.log(
      'Document Details - Loading document:',
      documentId
    );

    this.documentService
      .getDocuments()
      .pipe(
        finalize(() => {
          console.log(
            'Document Details - Request finished'
          );

          this.isLoading = false;
        })
      )
      .subscribe({
        next: (documents: DocumentItem[]) => {

          console.log(
            'Document Details - Documents received:',
            documents
          );

          const foundDocument = documents.find(
            item => item.documentId === documentId
          );

          if (foundDocument) {

            console.log(
              'Document Details - Document found:',
              foundDocument
            );

            this.document = foundDocument;

          } else {

            console.warn(
              'Document Details - Document not found:',
              documentId
            );

            this.errorMessage = 'Document not found.';
          }
        },

        error: (error) => {

          console.error(
            'Document Details - Failed to load documents:',
            error
          );

          this.document = null;
          this.errorMessage =
            'Unable to load document details.';
        }
      });
  }

  onDownload(): void {

    if (!this.document) {
      return;
    }

    this.documentService
      .downloadDocument(this.document.documentId)
      .subscribe({
        next: (blob) => {

          const url = window.URL.createObjectURL(blob);
          const link = window.document.createElement('a');

          link.href = url;
          link.download = this.document!.originalFileName;

          link.click();

          window.URL.revokeObjectURL(url);
        },

        error: (error) => {

          console.error(
            'Failed to download document:',
            error
          );

          this.errorMessage =
            'Unable to download document.';
        }
      });
  }

  onPromptDelete(): void {

    if (!this.document) {
      return;
    }

    this.showDeleteDialog = true;
  }

  confirmDelete(): void {

    if (!this.document) {
      return;
    }

    this.documentService
      .deleteDocument(this.document.documentId)
      .subscribe({
        next: () => {

          this.showDeleteDialog = false;

          this.router.navigate([
            '/vault/documents'
          ]);
        },

        error: (error) => {

          console.error(
            'Failed to delete document:',
            error
          );

          this.errorMessage =
            'Unable to delete document.';
        }
      });
  }

  cancelDelete(): void {
    this.showDeleteDialog = false;
  }
}