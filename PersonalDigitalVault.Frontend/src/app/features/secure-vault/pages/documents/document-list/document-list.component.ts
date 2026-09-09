import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { DocumentService } from '../../../services/document.service';
import { DocumentItem } from '../../../models/document.model';

import { DocumentCardComponent } from '../../../components/document-card/document-card.component';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../../shared/components/empty-state/empty-state.component';
import { ConfirmDialogComponent } from '../../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-document-list',
  standalone: true,
  imports: [
    RouterLink,
    DocumentCardComponent,
    LoadingComponent,
    EmptyStateComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './document-list.component.html',
  styleUrl: './document-list.component.css'
})
export class DocumentListComponent implements OnInit {
  private documentService = inject(DocumentService);

  documents: DocumentItem[] = [];

  isLoading = false;
  errorMessage = '';

  showDeleteDialog = false;
  selectedDocumentId: number | null = null;

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.documentService.getDocuments().subscribe({
      next: (documents) => {
        this.documents = documents;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to load documents:', error);
        this.documents = [];
        this.isLoading = false;
        this.errorMessage = 'Unable to load documents.';
      }
    });
  }

 onDownloadDocument(documentItem: DocumentItem): void {
  this.documentService.downloadDocument(documentItem.documentId).subscribe({
    next: (blob) => {
      const url = window.URL.createObjectURL(blob);
      const link = window.document.createElement('a');

      link.href = url;
      link.download = documentItem.originalFileName;
      link.click();

      window.URL.revokeObjectURL(url);
    },
    error: (error) => {
      console.error('Failed to download document:', error);
      this.errorMessage = 'Unable to download document.';
    }
  });
}
  onPromptDeleteDocument(documentId: number): void {
    this.selectedDocumentId = documentId;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {
    if (this.selectedDocumentId === null) {
      return;
    }

    const documentId = this.selectedDocumentId;

    this.documentService.deleteDocument(documentId).subscribe({
      next: () => {
        this.showDeleteDialog = false;
        this.selectedDocumentId = null;
        this.loadDocuments();
      },
      error: (error) => {
        console.error('Failed to delete document:', error);
        this.showDeleteDialog = false;
        this.selectedDocumentId = null;
        this.errorMessage = 'Unable to delete document.';
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteDialog = false;
    this.selectedDocumentId = null;
  }
}