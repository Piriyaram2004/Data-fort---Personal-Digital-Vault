import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
    folderId: [''],
    description: ['']
  });

  ngOnInit(): void {
    this.loadFolders();
  }

  loadFolders(): void {
    this.folderService.getFolders().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.folders = res.data;
        }
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
    if (this.uploadForm.value.folderId) {
      formData.append('folderId', this.uploadForm.value.folderId);
    }
    if (this.uploadForm.value.description) {
      formData.append('description', this.uploadForm.value.description);
    }

    this.documentService.uploadDocument(formData).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success) {
          this.router.navigate(['/vault/documents']);
        } else {
          this.errorMessage = res.message || 'Upload failed.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Error uploading file.';
      }
    });
  }
}
