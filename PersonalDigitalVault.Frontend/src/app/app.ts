import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SessionExpiredComponent } from './shared/components/session-expired/session-expired.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SessionExpiredComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('PersonalDigitalVault.Frontend');
}