import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../../../core/services/token.service';
import { SessionExpiredService } from '../../services/session-expired.service';

@Component({
  selector: 'app-session-expired',
  standalone: true,
  template: `
    @if (sessionExpiredService.isVisible()) {
      <div
        class="fixed inset-0 z-[9999] flex items-center justify-center
               bg-slate-950/75 backdrop-blur-sm p-4"
      >
        <div
          class="w-full max-w-md rounded-2xl border border-slate-600
                 bg-slate-800 p-8 text-center shadow-2xl"
        >
          <div
            class="mx-auto mb-5 flex h-16 w-16 items-center justify-center
                   rounded-2xl bg-blue-600 text-2xl font-bold text-white"
          >
            DF
          </div>

          <h2 class="text-2xl font-bold text-white">
            Your Session Has Expired
          </h2>

          <p class="mt-3 text-sm leading-6 text-slate-300">
            Your session has expired. Please log in again to continue.
          </p>

          <button
            type="button"
            (click)="loginAgain()"
            class="mt-7 w-full rounded-xl bg-blue-600 px-5 py-3
                   font-semibold text-white transition
                   hover:bg-blue-700"
          >
            Login Again
          </button>
        </div>
      </div>
    }
  `
})
export class SessionExpiredComponent {

  protected readonly sessionExpiredService = inject(SessionExpiredService);
  private readonly tokenService = inject(TokenService);
  private readonly router = inject(Router);

  loginAgain(): void {
    this.tokenService.clearToken();
    this.sessionExpiredService.hide();
    this.router.navigate(['/auth/login']);
  }
}