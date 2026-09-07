import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
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
  private route = inject(ActivatedRoute);

  documents: DocumentItem[] = [];
  isLoading = true;
  folderIdFilter: string | null = null;

  showDeleteDialog = false;
  selectedDocumentId: string | null = null;

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.folderIdFilter = params['folderId'] || null;
      this.loadDocuments();
    });
  }

  loadDocuments(): void {
    this.isLoading = true;
    this.documentService.getDocuments(this.folderIdFilter || undefined).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.documents = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
        this.documents = [];
      }
    });
  }

  onDownloadDocument(doc: DocumentItem): void {
    this.documentService.downloadDocument(doc.id).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = doc.fileName;
        a.click();
        window.URL.revokeObjectURL(url);
      }
    });
  }

  onPromptDeleteDocument(id: string): void {
    this.selectedDocumentId = id;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {
    if (!this.selectedDocumentId) return;
    this.documentService.deleteDocument(this.selectedDocumentId).subscribe({
      next: () => {
        this.showDeleteDialog = false;
        this.loadDocuments();
      },
      error: () => {
        this.showDeleteDialog = false;
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteDialog = false;
    this.selectedDocumentId = null;
  }
}
