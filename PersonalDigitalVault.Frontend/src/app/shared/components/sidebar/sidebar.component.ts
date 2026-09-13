import { NavigationService } from '../../../core/services/navigation.service';
import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { TokenService } from '../../../core/services/token.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  readonly navigation = inject(NavigationService);
  private tokenService = inject(TokenService);

  get isAdmin(): boolean {
    return this.tokenService.isAdmin();
  }
}
