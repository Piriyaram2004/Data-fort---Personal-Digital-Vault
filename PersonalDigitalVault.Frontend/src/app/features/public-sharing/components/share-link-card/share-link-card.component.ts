import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ShareLink } from '../../models/share-link.model';

@Component({
  selector: 'app-share-link-card',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './share-link-card.component.html',
  styleUrl: './share-link-card.component.css'
})
export class ShareLinkCardComponent {
  @Input() link!: ShareLink;
  @Output() revoke = new EventEmitter<string>();

  copied = false;

  get fullPublicUrl(): string {
    return `${window.location.origin}/public/file/${this.link.shareToken}`;
  }

  copyUrl(): void {
    navigator.clipboard.writeText(this.fullPublicUrl);
    this.copied = true;
    setTimeout(() => (this.copied = false), 2000);
  }

  onRevoke(): void {
    this.revoke.emit(this.link.id);
  }
}
