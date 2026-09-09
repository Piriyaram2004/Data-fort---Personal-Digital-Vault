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
  private readonly route = inject(ActivatedRoute);
  private readonly sharingService = inject(PublicSharingService);

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
    this.errorMessage = '';

    this.sharingService.getPublicFileDetails(token).subscribe({
      next: (fileDetails) => {
        this.fileDetails = fileDetails;
        this.isLoading = false;
      },
      error: () => {
        this.fileDetails = null;
        this.isLoading = false;
        this.errorMessage = 'Link is invalid, revoked, or expired.';
      }
    });
  }

  onDownloadPublicFile(): void {
    if (!this.shareToken) {
      return;
    }

    this.isDownloading = true;
    this.errorMessage = '';

    this.sharingService.downloadPublicFile(this.shareToken).subscribe({
      next: (blob) => {
        this.isDownloading = false;

        const url = window.URL.createObjectURL(blob);
        const anchor = document.createElement('a');

        anchor.href = url;
        anchor.download = this.fileDetails?.fileName || 'downloaded-file';

        anchor.click();

        window.URL.revokeObjectURL(url);
      },
      error: () => {
        this.isDownloading = false;
        this.errorMessage = 'Failed to download file.';
      }
    });
  }
}