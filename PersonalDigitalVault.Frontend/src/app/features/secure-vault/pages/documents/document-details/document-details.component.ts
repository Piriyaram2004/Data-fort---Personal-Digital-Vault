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

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadDocumentDetails(id);
    }
  }

  loadDocumentDetails(id: string): void {
    this.isLoading = true;
    this.documentService.getDocumentById(id).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.document = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onDownload(): void {
    if (!this.document) return;
    this.documentService.downloadDocument(this.document.id).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = this.document!.fileName;
        a.click();
        window.URL.revokeObjectURL(url);
      }
    });
  }

  confirmDelete(): void {
    if (!this.document) return;
    this.documentService.deleteDocument(this.document.id).subscribe({
      next: () => {
        this.showDeleteDialog = false;
        this.router.navigate(['/vault/documents']);
      }
    });
  }
}
