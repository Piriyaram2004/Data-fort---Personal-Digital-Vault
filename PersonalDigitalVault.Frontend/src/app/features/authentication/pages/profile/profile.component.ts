import { Component, OnInit, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserProfile } from '../../models/profile.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, DatePipe],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

  profile: UserProfile | null = null;
  isLoading = true;
  isSaving = false;
  successMessage = '';
  errorMessage = '';

  profileForm: FormGroup = this.fb.group({
    username: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]]
  });

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.authService.getProfile().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.profile = res.data;
          this.profileForm.patchValue({
            username: res.data.username,
            email: res.data.email
          });
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load user profile.';
      }
    });
  }

  onSaveProfile(): void {
    if (this.profileForm.invalid) return;

    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.authService.updateProfile(this.profileForm.value).subscribe({
      next: (res) => {
        this.isSaving = false;
        if (res.success) {
          this.successMessage = 'Profile updated successfully.';
        } else {
          this.errorMessage = res.message || 'Failed to update profile.';
        }
      },
      error: (err) => {
        this.isSaving = false;
        this.errorMessage = err.error?.message || 'Error saving profile.';
      }
    });
  }
}
