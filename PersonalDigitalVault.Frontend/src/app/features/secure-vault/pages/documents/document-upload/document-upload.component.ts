import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { DocumentService } from '../../../services/document.service';
import { FolderService } from '../../../services/folder.service';
import { Folder } from '../../../models/folder.model';

@Component({
  selector: 'app-document-upload',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './document-upload.component.html',
  styleUrl: './document-upload.component.css'
})
export class DocumentUploadComponent implements OnInit {
  private fb = inject(FormBuilder);
  private documentService = inject(DocumentService);
  private folderService = inject(FolderService);
  private router = inject(Router);

  folders: Folder[] = [];
  selectedFile: File | null = null;

  isLoading = false;
  errorMessage = '';

  uploadForm: FormGroup = this.fb.group({
    folderId: ['']
  });

  ngOnInit(): void {
    this.loadFolders();
  }

  loadFolders(): void {
    this.folderService.getFolders().subscribe({
      next: (folders) => {
        this.folders = folders;
      },
      error: () => {
        this.folders = [];
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  onSubmit(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'Please select a file to upload.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formData = new FormData();

    formData.append('file', this.selectedFile);

    const folderId = this.uploadForm.value.folderId;

    if (folderId) {
      formData.append('folderId', folderId);
    }

    this.documentService.uploadDocument(formData).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/vault/documents']);
      },
      error: (error) => {
        this.isLoading = false;

        console.error('Failed to upload document:', error);

        this.errorMessage =
          error.error?.message || 'Error uploading file.';
      }
    });
  }
}