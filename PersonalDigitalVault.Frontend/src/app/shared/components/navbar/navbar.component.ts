import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { TokenService } from '../../../core/services/token.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private tokenService = inject(TokenService);
  private router = inject(Router);

  get userEmail(): string | null {
    return this.tokenService.getUserEmail();
  }

  get userName(): string | null {
    return this.tokenService.getUserName();
  }

  get userRole(): string | null {
    return this.tokenService.getUserRole();
  }

  get isAdmin(): boolean {
    return this.tokenService.isAdmin();
  }

  get displayUserName(): string {
    const name = this.userName;

    if (!name) return 'User';

    return name.charAt(0).toUpperCase() + name.slice(1);
  }

  get userInitials(): string {
    const name = this.userName;

    if (!name) return 'U';

    return name.substring(0, 2).toUpperCase();
  }

  logout(): void {
    this.tokenService.clearToken();
    this.router.navigate(['/auth/login']);
  }
}