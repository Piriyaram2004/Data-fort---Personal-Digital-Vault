import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FolderService } from '../../services/folder.service';
import { DocumentService } from '../../services/document.service';
import { CredentialService } from '../../services/credential.service';
import { Folder } from '../../models/folder.model';
import { DocumentItem } from '../../models/document.model';
import { CredentialItem } from '../../models/credential.model';
import { FolderCardComponent } from '../../components/folder-card/folder-card.component';
import { DocumentCardComponent } from '../../components/document-card/document-card.component';
import { CredentialCardComponent } from '../../components/credential-card/credential-card.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-vault-home',
  standalone: true,
  imports: [
    RouterLink,
    FolderCardComponent,
    DocumentCardComponent,
    CredentialCardComponent,
    LoadingComponent
  ],
  templateUrl: './vault-home.component.html',
  styleUrl: './vault-home.component.css'
})
export class VaultHomeComponent implements OnInit {
  private folderService = inject(FolderService);
  private documentService = inject(DocumentService);
  private credentialService = inject(CredentialService);

  isLoading = true;
  recentFolders: Folder[] = [];
  recentDocuments: DocumentItem[] = [];
  recentCredentials: CredentialItem[] = [];

  ngOnInit(): void {
    this.loadOverview();
  }

  loadOverview(): void {
    this.isLoading = true;
    this.folderService.getFolders().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.recentFolders = res.data.slice(0, 3);
        }
      },
      error: () => {
        this.recentFolders = [];
      }
    });

    this.documentService.getDocuments().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.recentDocuments = res.data.slice(0, 3);
        }
      },
      error: () => {
        this.recentDocuments = [];
      }
    });

    this.credentialService.getCredentials().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.recentCredentials = res.data.slice(0, 3);
        }
      },
      error: () => {
        this.isLoading = false;
        this.recentCredentials = [];
      }
    });
  }
}
