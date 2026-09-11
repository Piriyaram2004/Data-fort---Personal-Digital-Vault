import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly TOKEN_KEY = 'auth_token';

  // Used to notify services when the token changes.
  readonly tokenVersion = signal(0);

  getToken(): string | null {
    return sessionStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    sessionStorage.setItem(this.TOKEN_KEY, token);
    this.tokenVersion.update(value => value + 1);
  }

  clearToken(): void {
    sessionStorage.removeItem(this.TOKEN_KEY);
    this.tokenVersion.update(value => value + 1);
  }

  hasToken(): boolean {
    return !!this.getToken();
  }

  getUserRole(): string | null {
    const payload = this.getJwtPayload();
    if (!payload) return null;

    return payload.role ||
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
      null;
  }

  getUserEmail(): string | null {
    const payload = this.getJwtPayload();
    if (!payload) return null;

    return payload.email ||
      payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ||
      null;
  }

  getUserName(): string | null {
    const payload = this.getJwtPayload();
    if (!payload) return null;

    return payload.name ||
      payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
      null;
  }

  isAdmin(): boolean {
  const role = this.getUserRole();
  return role?.toLowerCase() === 'administrator';
}

  getTokenExpiryTime(): number | null {
    const payload = this.getJwtPayload();

    if (!payload || typeof payload.exp !== 'number') {
      return null;
    }

    // JWT exp is in seconds. JavaScript Date.now() is in milliseconds.
    return payload.exp * 1000;
  }

  private getJwtPayload(): any {
    const token = this.getToken();

    if (!token) return null;

    try {
      const parts = token.split('.');

      if (parts.length !== 3) return null;

      const payload = atob(
        parts[1]
          .replace(/-/g, '+')
          .replace(/_/g, '/')
      );

      return JSON.parse(payload);
    } catch {
      return null;
    }
  }
}