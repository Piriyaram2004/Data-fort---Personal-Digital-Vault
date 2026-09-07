import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FolderService } from '../../../services/folder.service';

@Component({
  selector: 'app-folder-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './folder-form.component.html',
  styleUrl: './folder-form.component.css'
})
export class FolderFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private folderService = inject(FolderService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isEditMode = false;
  folderId: string | null = null;
  isLoading = false;
  errorMessage = '';

  folderForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    description: ['']
  });

  ngOnInit(): void {
    this.folderId = this.route.snapshot.paramMap.get('id');
    if (this.folderId) {
      this.isEditMode = true;
      this.loadFolderDetails(this.folderId);
    }
  }

  loadFolderDetails(id: string): void {
    this.isLoading = true;
    this.folderService.getFolderById(id).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.folderForm.patchValue({
            name: res.data.name,
            description: res.data.description
          });
        }
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.folderForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const payload = this.folderForm.value;

    if (this.isEditMode && this.folderId) {
      this.folderService.updateFolder(this.folderId, payload).subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(['/vault/folders']);
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Error updating folder.';
        }
      });
    } else {
      this.folderService.createFolder(payload).subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(['/vault/folders']);
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Error creating folder.';
        }
      });
    }
  }
}
