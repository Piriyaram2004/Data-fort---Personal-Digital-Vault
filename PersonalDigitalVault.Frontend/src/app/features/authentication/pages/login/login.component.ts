import {
  Component,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../../../core/services/token.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private tokenService = inject(TokenService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  email = '';
  password = '';
  errorMessage = '';
  isLoading = false;
  showPassword = false;

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onLogin(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'Please provide both email and password.';
      this.cdr.detectChanges();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login({
      email: this.email,
      password: this.password
    }).subscribe({
      next: (res) => {
        this.isLoading = false;

        if (res.token) {
  this.tokenService.setToken(res.token);
  if (this.tokenService.isAdmin()) {
    this.router.navigate(['/admin/dashboard']);
    } else {
    this.router.navigate(['/vault']);
    }
    } else {
          this.errorMessage = 'Login failed.';
          this.cdr.detectChanges();
        }
      },

      error: (err) => {
        this.isLoading = false;

        this.errorMessage =
          err.error?.message || 'Server error during login.';

        this.cdr.detectChanges();
      }
    });
  }
}