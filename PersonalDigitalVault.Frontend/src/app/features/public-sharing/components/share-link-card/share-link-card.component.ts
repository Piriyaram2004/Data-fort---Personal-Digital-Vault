import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  signal
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { ShareLink } from '../../models/share-link.model';

@Component({
  selector: 'app-share-link-card',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './share-link-card.component.html',
  styleUrl: './share-link-card.component.css'
})
export class ShareLinkCardComponent implements OnDestroy {
  @Input() link!: ShareLink;

  @Output() revoke = new EventEmitter<number>();
  @Output() delete = new EventEmitter<number>();

  copied = false;

  /**
   * Small live clock used only to refresh the expiry state.
   *
   * The backend remains the authority for actual public access.
   * This signal is only for updating the owner's UI automatically.
   */
  private readonly currentTime = signal(Date.now());

  private readonly expiryTimer = window.setInterval(() => {
    this.currentTime.set(Date.now());
  }, 1000);

  /**
   * Backend stores ExpiresAt as UTC.
   *
   * Current API response does not include a timezone suffix,
   * for example:
   *
   * 2026-09-11T14:06:00
   *
   * Therefore we explicitly treat it as UTC.
   */
  private get expiryDate(): Date | null {
    if (!this.link.expiresAt) {
      return null;
    }

    const value = this.link.expiresAt;

    // If the API already provides timezone information,
    // use the value directly.
    if (
      value.endsWith('Z') ||
      value.includes('+') ||
      value.match(/-\d{2}:\d{2}$/)
    ) {
      const date = new Date(value);

      return Number.isNaN(date.getTime())
        ? null
        : date;
    }

    // Current API returns UTC without a timezone suffix.
    const date = new Date(`${value}Z`);

    return Number.isNaN(date.getTime())
      ? null
      : date;
  }

  /**
   * True when the link has naturally expired.
   */
  get isExpired(): boolean {
    // Reading the signal makes Angular track this value.
    const now = this.currentTime();

    const expiry = this.expiryDate;

    if (!expiry) {
      return false;
    }

    return expiry.getTime() <= now;
  }

  /**
   * A link is active only when it is:
   * - not manually revoked
   * - not naturally expired
   */
  get isActive(): boolean {
    return !this.link.isRevoked && !this.isExpired;
  }

  /**
   * Public share URL.
   */
  get fullPublicUrl(): string {
    return `${window.location.origin}/public/file/${this.link.shareToken}`;
  }

  /**
   * Expiry date displayed in the user's local timezone.
   */
  get displayExpiryDate(): Date | null {
    return this.expiryDate;
  }

  /**
   * Copy the public URL only while the link is active.
   */
  copyUrl(): void {
    if (!this.isActive) {
      return;
    }

    navigator.clipboard.writeText(this.fullPublicUrl);

    this.copied = true;

    setTimeout(() => {
      this.copied = false;
    }, 2000);
  }

  /**
   * Revoke only while the link is active.
   */
  onRevoke(): void {
    if (!this.isActive) {
      return;
    }

    this.revoke.emit(this.link.shareLinkId);
  }

  /**
   * Delete the share link.
   *
   * Delete is allowed regardless of the link state:
   * - Active
   * - Expired
   * - Revoked
   */
  onDelete(): void {
    this.delete.emit(this.link.shareLinkId);
  }

  /**
   * Stop the timer when the card is destroyed.
   */
  ngOnDestroy(): void {
    window.clearInterval(this.expiryTimer);
  }
}