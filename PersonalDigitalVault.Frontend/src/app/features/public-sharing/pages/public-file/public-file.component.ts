import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { PublicSharingService } from '../../services/public-sharing.service';
import { PublicFileDetails } from '../../models/share-link.model';
import { FileSizePipe } from '../../../../shared/pipes/file-size.pipe';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-public-file',
  standalone: true,
  imports: [DatePipe, FileSizePipe, LoadingComponent],
  templateUrl: './public-file.component.html',
  styleUrl: './public-file.component.css'
})
export class PublicFileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private sharingService = inject(PublicSharingService);

  shareToken: string | null = null;
  fileDetails: PublicFileDetails | null = null;
  isLoading = true;
  isDownloading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.shareToken = this.route.snapshot.paramMap.get('token');
    if (this.shareToken) {
      this.loadPublicFileDetails(this.shareToken);
    } else {
      this.isLoading = false;
      this.errorMessage = 'Invalid public share link.';
    }
  }

  loadPublicFileDetails(token: string): void {
    this.isLoading = true;
    this.sharingService.getPublicFileDetails(token).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.fileDetails = res.data;
        } else {
          this.errorMessage = res.message || 'Share link expired or revoked.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Link is invalid, revoked, or expired.';
      }
    });
  }

  onDownloadPublicFile(): void {
    if (!this.shareToken) return;

    this.isDownloading = true;
    this.sharingService.downloadPublicFile(this.shareToken).subscribe({
      next: (blob) => {
        this.isDownloading = false;
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = this.fileDetails?.documentName || 'downloaded-file';
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: () => {
        this.isDownloading = false;
        this.errorMessage = 'Failed to download file.';
      }
    });
  }
}
