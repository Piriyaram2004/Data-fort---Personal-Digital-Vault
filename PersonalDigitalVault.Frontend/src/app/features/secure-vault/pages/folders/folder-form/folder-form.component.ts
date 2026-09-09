import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';

import { FolderService } from '../../../services/folder.service';

import {
  CreateFolderRequest,
  UpdateFolderRequest
} from '../../../models/folder.model';

@Component({
  selector: 'app-folder-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './folder-form.component.html',
  styleUrl: './folder-form.component.css'
})
export class FolderFormComponent implements OnInit {

  private fb = inject(FormBuilder);
  private folderService = inject(FolderService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  folderId: number | null = null;

  isEditMode = false;
  isLoading = false;

  errorMessage = '';

  folderForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(255)]],
    description: ['']
  });

  ngOnInit(): void {

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {

      this.folderId = Number(id);
      this.isEditMode = true;

      this.loadFolder();
    }
  }

  loadFolder(): void {

    if (this.folderId === null) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.folderService.getFolders().subscribe({

      next: (response) => {

        this.isLoading = false;

        const folder = response.find(
          f => f.folderId === this.folderId
        );

        if (!folder) {

          this.errorMessage = 'Folder not found.';
          return;
        }

        this.folderForm.patchValue({
          name: folder.folderName,
          description: folder.description ?? ''
        });
      },

      error: () => {

        this.isLoading = false;
        this.errorMessage =
          'Unable to load folder.';
      }
    });
  }

  onSubmit(): void {

    if (this.folderForm.invalid) {

      this.folderForm.markAllAsTouched();
      return;
    }

    this.saveFolder();
  }

  saveFolder(): void {

    this.errorMessage = '';

    const formValue = this.folderForm.getRawValue();

    const folderName = formValue.name.trim();

    const description =
      formValue.description.trim() || null;

    if (!folderName) {

      this.errorMessage =
        'Folder name is required.';

      return;
    }

    this.isLoading = true;

    if (
      this.isEditMode &&
      this.folderId !== null
    ) {

      const payload: UpdateFolderRequest = {
        folderName,
        description
      };

      this.folderService
        .updateFolder(
          this.folderId,
          payload
        )
        .subscribe({

          next: () => {

            this.router.navigate([
              '/vault/folders'
            ]);
          },

          error: (error) => {

            this.isLoading = false;

            if (error.status === 409) {

              this.errorMessage =
                'A folder with this name already exists.';

            } else {

              this.errorMessage =
                'Unable to update folder.';
            }
          }
        });

    } else {

      const payload: CreateFolderRequest = {
        folderName,
        parentFolderId: null,
        description
      };

      this.folderService
        .createFolder(payload)
        .subscribe({

          next: () => {

            this.router.navigate([
              '/vault/folders'
            ]);
          },

          error: (error) => {

            this.isLoading = false;

            if (error.status === 409) {

              this.errorMessage =
                'A folder with this name already exists.';

            } else {

              this.errorMessage =
                'Unable to create folder.';
            }
          }
        });
    }
  }
}