import { Injectable, inject, signal, DestroyRef } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
@Injectable({ providedIn: 'root' })
export class NavigationService {
  readonly open = signal(false);
  readonly collapsed = signal(false);
  private returnFocus: HTMLElement | null = null;
  constructor() {
    inject(Router)
      .events.pipe(takeUntilDestroyed(inject(DestroyRef)))
      .subscribe((event) => {
        if (event instanceof NavigationEnd) this.close();
      });
  }
  show(): void {
    this.returnFocus = document.activeElement as HTMLElement;
    this.open.set(true);
    document.body.style.overflow = 'hidden';
    setTimeout(() => document.querySelector<HTMLButtonElement>('.drawer-close')?.focus());
  }
  close(): void {
    if (!this.open()) return;
    this.open.set(false);
    document.body.style.overflow = '';
    const trigger = this.returnFocus;
    setTimeout(() => trigger?.focus());
  }
  toggleCollapsed(): void {
    this.collapsed.update((value) => !value);
  }
}
