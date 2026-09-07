import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading',
  standalone: true,
  template: `
    <div class="flex flex-col items-center justify-center p-6 text-gray-600">
      <div class="w-10 h-10 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"></div>
      <p class="mt-3 text-sm font-medium">{{ message }}</p>
    </div>
  `
})
export class LoadingComponent {
  @Input() message: string = 'Loading...';
}
