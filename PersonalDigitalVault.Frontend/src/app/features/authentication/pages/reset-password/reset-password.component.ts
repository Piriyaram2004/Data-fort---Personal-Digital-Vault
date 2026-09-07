import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  errorMessage = '';
  isLoading = false;

  resetForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    token: ['', [Validators.required]],
    newPassword: ['', [Validators.required, Validators.minLength(6)]]
  });

  onReset(): void {
    if (this.resetForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const { email, token, newPassword } = this.resetForm.value;
    this.authService.resetPassword(email, token, newPassword).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success) {
          this.router.navigate(['/auth/login']);
        } else {
          this.errorMessage = res.message || 'Password reset failed.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Error resetting password.';
      }
    });
  }
}
