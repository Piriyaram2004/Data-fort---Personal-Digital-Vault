import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  template: `
    <div class="flex flex-col items-center justify-center p-8 text-center bg-gray-50 rounded-xl border border-dashed border-gray-300">
      <div class="w-12 h-12 text-gray-400 mb-3">
        <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
        </svg>
      </div>
      <h3 class="text-base font-semibold text-gray-900">{{ title }}</h3>
      <p class="mt-1 text-sm text-gray-500 max-w-sm">{{ message }}</p>
    </div>
  `
})
export class EmptyStateComponent {
  @Input() title: string = 'No Items Found';
  @Input() message: string = 'There are no items to display at the moment.';
}
