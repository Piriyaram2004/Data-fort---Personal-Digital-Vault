import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { RouterLink } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { UserProfile } from '../../models/profile.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  profile: UserProfile | null = null;

  isLoading = true;
  isSaving = false;
  isUpdated = false;

  successMessage = '';
  errorMessage = '';

  profileForm: FormGroup = this.fb.group({
    username: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    fullName: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.getProfile().subscribe({
      next: (res) => {
        this.profile = res;

        this.profileForm.patchValue({
          username: res.userName,
          email: res.email,
          fullName: res.fullName
        });

        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (err) => {
        this.isLoading = false;

        this.errorMessage =
          err.error?.message || 'Failed to load user profile.';

        this.cdr.detectChanges();
      }
    });
  }

  onSaveProfile(): void {
  if (this.profileForm.invalid) {
    this.profileForm.markAllAsTouched();
    return;
  }

  this.isSaving = true;
  this.successMessage = '';
  this.errorMessage = '';

  this.authService.updateProfile(this.profileForm.value).subscribe({
    next: (res) => {
      this.isSaving = false;

      this.profile = res;

      this.successMessage = 'Profile updated successfully.';
      this.isUpdated = true;

      this.profileForm.patchValue({
        username: res.userName,
        fullName: res.fullName,
        email: res.email
      });

      this.cdr.detectChanges();
    },

    error: (err) => {
      this.isSaving = false;

      this.errorMessage =
        err.error?.message || 'Error saving profile.';

      this.cdr.detectChanges();
    }
  });
  }
}