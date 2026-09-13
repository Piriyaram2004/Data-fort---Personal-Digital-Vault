import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { CredentialService } from '../../../services/credential.service';

import {
  CredentialItem,
  CreateCredentialRequest,
  UpdateCredentialRequest
} from '../../../models/credential.model';

@Component({
  selector: 'app-credential-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './credential-form.component.html',
  styleUrl: './credential-form.component.css'
})
export class CredentialFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private credentialService = inject(CredentialService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isEditMode = false;

  credentialId: number | null = null;

  isLoading = false;
  errorMessage = '';

  credentialForm: FormGroup = this.fb.group({
    folderId: [null],
    title: ['', [Validators.required]],
    userName: ['', [Validators.required]],
    password: ['', [Validators.required]],
    notes: ['']
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      return;
    }

    const parsedId = Number(id);

    if (!Number.isInteger(parsedId) || parsedId <= 0) {
      this.errorMessage = 'Invalid credential ID.';
      return;
    }

    this.credentialId = parsedId;
    this.isEditMode = true;

    this.loadCredentialDetails(parsedId);
  }

  loadCredentialDetails(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.credentialService.getCredentials().subscribe({
      next: (credentials: CredentialItem[]) => {
        const credential = credentials.find(
          item => item.credentialId === id
        );

        if (!credential) {
          this.errorMessage = 'Credential not found.';
          this.isLoading = false;
          return;
        }

        this.credentialForm.patchValue({
          folderId: credential.folderId,
          title: credential.title,
          userName: credential.userName,
          password: credential.password,
          notes: credential.notes ?? ''
        });

        this.isLoading = false;
      },

      error: (error) => {
        console.error(
          'Failed to load credential:',
          error
        );

        this.isLoading = false;
        this.errorMessage = 'Unable to load credential.';
      }
    });
  }

  onSubmit(): void {
    if (this.credentialForm.invalid) {
      this.credentialForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formValue = this.credentialForm.getRawValue();

    if (this.isEditMode && this.credentialId !== null) {
      const updateRequest: UpdateCredentialRequest = {
        folderId: formValue.folderId ?? null,
        title: formValue.title,
        userName: formValue.userName,
        password: formValue.password,
        notes: formValue.notes || null
      };

      this.credentialService
        .updateCredential(this.credentialId, updateRequest)
        .subscribe({
          next: () => {
            this.isLoading = false;
            this.router.navigate(['/vault/credentials']);
          },

          error: (error) => {
            console.error(
              'Failed to update credential:',
              error
            );

            this.isLoading = false;
            this.errorMessage =
              error.error?.message ||
              'Error updating credential.';
          }
        });

      return;
    }

    const createRequest: CreateCredentialRequest = {
      folderId: formValue.folderId ?? null,
      title: formValue.title,
      userName: formValue.userName,
      password: formValue.password,
      notes: formValue.notes || null
    };

    this.credentialService
      .createCredential(createRequest)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(['/vault/credentials']);
        },

        error: (error) => {
          console.error(
            'Failed to create credential:',
            error
          );

          this.isLoading = false;
          this.errorMessage =
            error.error?.message ||
            'Error creating credential.';
        }
      });
  }
}