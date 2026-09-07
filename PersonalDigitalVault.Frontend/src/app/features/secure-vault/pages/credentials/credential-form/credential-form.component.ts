import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CredentialService } from '../../../services/credential.service';

@Component({
  selector: 'app-credential-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './credential-form.component.html',
  styleUrl: './credential-form.component.css'
})
export class CredentialFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private credentialService = inject(CredentialService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isEditMode = false;
  credentialId: string | null = null;
  isLoading = false;
  errorMessage = '';

  credentialForm: FormGroup = this.fb.group({
    serviceName: ['', [Validators.required]],
    username: ['', [Validators.required]],
    secretValue: ['', [Validators.required]],
    notes: ['']
  });

  ngOnInit(): void {
    this.credentialId = this.route.snapshot.paramMap.get('id');
    if (this.credentialId) {
      this.isEditMode = true;
      this.loadCredentialDetails(this.credentialId);
    }
  }

  loadCredentialDetails(id: string): void {
    this.isLoading = true;
    this.credentialService.getCredentialById(id).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.credentialForm.patchValue({
            serviceName: res.data.serviceName,
            username: res.data.username,
            secretValue: res.data.secretValue || '',
            notes: res.data.notes || ''
          });
        }
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.credentialForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const payload = this.credentialForm.value;

    if (this.isEditMode && this.credentialId) {
      this.credentialService.updateCredential(this.credentialId, payload).subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(['/vault/credentials']);
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Error updating credential.';
        }
      });
    } else {
      this.credentialService.createCredential(payload).subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(['/vault/credentials']);
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Error creating credential.';
        }
      });
    }
  }
}
