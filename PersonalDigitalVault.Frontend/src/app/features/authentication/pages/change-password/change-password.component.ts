import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

  successMessage = '';
  errorMessage = '';
  isLoading = false;

  changeForm: FormGroup = this.fb.group({
    currentPassword: ['', [Validators.required]],
    newPassword: ['', [Validators.required, Validators.minLength(6)]]
  });

  onChangePassword(): void {
    if (this.changeForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const { currentPassword, newPassword } = this.changeForm.value;
    this.authService.changePassword(currentPassword, newPassword).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success) {
          this.successMessage = res.message || 'Password changed successfully.';
          this.changeForm.reset();
        } else {
          this.errorMessage = res.message || 'Change password failed.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Error updating password.';
      }
    });
  }
}
