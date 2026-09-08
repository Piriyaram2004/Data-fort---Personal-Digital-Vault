import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  successMessage = '';
  errorMessage = '';
  isLoading = false;

  forgotForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });

  onSubmit(): void {
    if (this.forgotForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

this.authService.forgotPassword(this.forgotForm.value.email).subscribe({
  next: () => {
    this.isLoading = false;
    this.successMessage =
      'If an account exists for this email, password reset instructions will be sent.';

    this.cdr.detectChanges();
  },
  error: () => {
    this.isLoading = false;
    this.errorMessage =
      'Unable to process the password reset request. Please try again.';

    this.cdr.detectChanges();
  }
});
  }
}
