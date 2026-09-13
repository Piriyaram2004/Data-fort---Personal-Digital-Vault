import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vault-home',
  standalone: true,
  templateUrl: './vault-home.component.html'
})
export class VaultHomeComponent {
  constructor(private router: Router) {}

  openFolders(): void {
    this.router.navigate(['/vault/folders']);
  }

  openDocuments(): void {
    this.router.navigate(['/vault/documents']);
  }

  openCredentials(): void {
    this.router.navigate(['/vault/credentials']);
  }

  openSearch(): void {
    this.router.navigate(['/vault/search']);
  }
}