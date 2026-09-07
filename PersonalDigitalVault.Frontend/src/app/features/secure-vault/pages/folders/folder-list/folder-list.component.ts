import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FolderService } from '../../../services/folder.service';
import { Folder } from '../../../models/folder.model';
import { FolderCardComponent } from '../../../components/folder-card/folder-card.component';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../../shared/components/empty-state/empty-state.component';
import { ConfirmDialogComponent } from '../../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-folder-list',
  standalone: true,
  imports: [
    RouterLink,
    FolderCardComponent,
    LoadingComponent,
    EmptyStateComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './folder-list.component.html',
  styleUrl: './folder-list.component.css'
})
export class FolderListComponent implements OnInit {
  private folderService = inject(FolderService);
  private router = inject(Router);

  folders: Folder[] = [];
  isLoading = true;

  showDeleteDialog = false;
  selectedFolderId: string | null = null;

  ngOnInit(): void {
    this.loadFolders();
  }

  loadFolders(): void {
    this.isLoading = true;
    this.folderService.getFolders().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.folders = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
        this.folders = [];
      }
    });
  }

  onEditFolder(folder: Folder): void {
    this.router.navigate(['/vault/folders/edit', folder.id]);
  }

  onPromptDeleteFolder(id: string): void {
    this.selectedFolderId = id;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {
    if (!this.selectedFolderId) return;
    this.folderService.deleteFolder(this.selectedFolderId).subscribe({
      next: () => {
        this.showDeleteDialog = false;
        this.loadFolders();
      },
      error: () => {
        this.showDeleteDialog = false;
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteDialog = false;
    this.selectedFolderId = null;
  }
}
