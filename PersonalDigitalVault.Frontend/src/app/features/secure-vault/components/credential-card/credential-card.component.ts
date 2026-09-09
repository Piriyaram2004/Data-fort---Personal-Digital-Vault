import { Component, EventEmitter, Input, Output } from '@angular/core';

import { CredentialItem } from '../../models/credential.model';

@Component({
  selector: 'app-credential-card',
  standalone: true,
  templateUrl: './credential-card.component.html',
  styleUrl: './credential-card.component.css'
})
export class CredentialCardComponent {
  @Input() credential!: CredentialItem;

  @Output() edit = new EventEmitter<CredentialItem>();
  @Output() delete = new EventEmitter<number>();

  showSecret = false;
  copied = false;

  toggleSecret(): void {
    this.showSecret = !this.showSecret;
  }

  copyUsername(): void {
    navigator.clipboard.writeText(this.credential.userName);

    this.copied = true;

    setTimeout(() => {
      this.copied = false;
    }, 2000);
  }

  onEdit(): void {
    this.edit.emit(this.credential);
  }

  onDelete(): void {
    this.delete.emit(this.credential.credentialId);
  }
}