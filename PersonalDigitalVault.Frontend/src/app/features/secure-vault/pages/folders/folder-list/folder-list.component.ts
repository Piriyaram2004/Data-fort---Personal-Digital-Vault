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

  isLoading = false;

  errorMessage = '';

  showDeleteDialog = false;

  selectedFolderId: number | null = null;

  ngOnInit(): void {
    this.loadFolders();
  }

  loadFolders(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.folderService.getFolders().subscribe({
      next: (folders: Folder[]) => {
        this.folders = folders;
        this.isLoading = false;
      },

      error: (error) => {
        console.error('Failed to load folders:', error);

        this.folders = [];
        this.isLoading = false;
        this.errorMessage = 'Unable to load folders.';
      }
    });
  }

  onEditFolder(folder: Folder): void {

    this.router.navigate([
      '/vault/folders/edit',
      folder.folderId
    ]);
  }

  onPromptDeleteFolder(id: number): void {

    this.selectedFolderId = id;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {

    if (this.selectedFolderId === null) {
      return;
    }

    const folderId = this.selectedFolderId;

    this.folderService
      .deleteFolder(folderId)
      .subscribe({

        next: () => {

          this.showDeleteDialog = false;
          this.selectedFolderId = null;

          this.loadFolders();
        },

        error: (error) => {

          console.error(
            'Failed to delete folder:',
            error
          );

          this.showDeleteDialog = false;
          this.selectedFolderId = null;

          this.errorMessage =
            'Unable to delete folder.';
        }
      });
  }

  cancelDelete(): void {

    this.showDeleteDialog = false;
    this.selectedFolderId = null;
  }
}