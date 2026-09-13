import { Injectable, signal, effect } from '@angular/core';
import { TokenService } from '../../core/services/token.service';

@Injectable({
  providedIn: 'root'
})
export class SessionExpiredService {
  readonly isVisible = signal(false);

  private expiryTimer: ReturnType<typeof setTimeout> | null = null;

  constructor(private readonly tokenService: TokenService) {
    effect(() => {
      this.tokenService.tokenVersion();

      this.scheduleExpiry();
    });
  }

  show(): void {
    this.isVisible.set(true);
  }

  hide(): void {
    this.isVisible.set(false);
  }

  private scheduleExpiry(): void {
    this.clearExpiryTimer();

    const token = this.tokenService.getToken();

    if (!token) {
      return;
    }

    const expiryTime = this.tokenService.getTokenExpiryTime();

    if (expiryTime === null) {
      return;
    }

    const remainingTime = expiryTime - Date.now();

    if (remainingTime <= 0) {
      this.show();
      return;
    }

    this.expiryTimer = setTimeout(() => {
      if (this.tokenService.hasToken()) {
        this.show();
      }
    }, remainingTime);
  }

  private clearExpiryTimer(): void {
    if (this.expiryTimer !== null) {
      clearTimeout(this.expiryTimer);
      this.expiryTimer = null;
    }
  }
}