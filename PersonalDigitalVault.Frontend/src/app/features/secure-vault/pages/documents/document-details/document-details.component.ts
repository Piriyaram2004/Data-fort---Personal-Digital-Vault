import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';

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
    LoadingComponent,
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
  showDeleteDialog = false;

  integrityVerified: boolean | null = null;
  integrityMessage = '';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.isLoading = false;
      return;
    }

    this.loadDocumentDetails(Number(id));
  }

  loadDocumentDetails(id: number): void {
    this.isLoading = true;

    this.documentService.getDocuments().subscribe({
      next: (documents: DocumentItem[]) => {
        this.document = documents.find(
          item => item.documentId === id
        ) ?? null;

        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to load document:', error);
        this.document = null;
        this.isLoading = false;
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
          console.error('Failed to download document:', error);
        }
      });
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
          this.router.navigate(['/vault/documents']);
        },
        error: (error) => {
          console.error('Failed to delete document:', error);
        }
      });
  }
}