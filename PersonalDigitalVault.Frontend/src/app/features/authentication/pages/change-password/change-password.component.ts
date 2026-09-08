import { ChangeDetectorRef, Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  successMessage = '';
  errorMessage = '';
  isLoading = false;

  showCurrentPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  changeForm: FormGroup = this.fb.group(
    {
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    },
    {
      validators: this.passwordMatchValidator
    }
  );

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const newPassword = control.get('newPassword')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    return newPassword === confirmPassword
      ? null
      : { passwordMismatch: true };
  }

  toggleCurrentPasswordVisibility(): void {
    this.showCurrentPassword = !this.showCurrentPassword;
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onChangePassword(): void {
    if (this.changeForm.invalid) {
      this.changeForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const {
      currentPassword,
      newPassword,
      confirmPassword
    } = this.changeForm.value;

    this.authService.changePassword(
  currentPassword,
  newPassword,
  confirmPassword
).subscribe({
  next: () => {
    this.isLoading = false;
    this.successMessage = 'Password changed successfully.';
    this.changeForm.reset();

    this.showCurrentPassword = false;
    this.showNewPassword = false;
    this.showConfirmPassword = false;

    this.cdr.detectChanges();
  },
  error: (err) => {
    this.isLoading = false;
    this.errorMessage =
      err.error?.message || 'Error updating password.';

    this.cdr.detectChanges();
  }
});
  }
}