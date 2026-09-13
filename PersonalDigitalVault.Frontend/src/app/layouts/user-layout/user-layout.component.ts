import { NavigationService } from '../../core/services/navigation.service';
import { Component, inject, HostListener } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-user-layout',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, SidebarComponent],
  templateUrl: './user-layout.component.html',
  styleUrl: './user-layout.component.css',
})
export class UserLayoutComponent {
  readonly navigation = inject(NavigationService);
  @HostListener('window:resize') onResize(): void {
    if (window.innerWidth >= 768) this.navigation.close();
  }
  @HostListener('document:keydown', ['$event']) onKey(event: KeyboardEvent): void {
    if (!this.navigation.open()) return;
    if (event.key === 'Escape') {
      this.navigation.close();
      return;
    }
    if (event.key !== 'Tab') return;
    const drawer = document.getElementById('vault-navigation');
    const elements = Array.from(
      drawer?.querySelectorAll<HTMLElement>('a[href],button:not([disabled])') || [],
    ).filter((el) => el.offsetParent !== null);
    const first = elements[0],
      last = elements[elements.length - 1];
    if (event.shiftKey && document.activeElement === first) {
      event.preventDefault();
      last?.focus();
    }
    if (!event.shiftKey && document.activeElement === last) {
      event.preventDefault();
      first?.focus();
    }
  }
}
