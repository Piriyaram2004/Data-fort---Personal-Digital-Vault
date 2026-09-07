import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-integrity-badge',
  standalone: true,
  templateUrl: './integrity-badge.component.html',
  styleUrl: './integrity-badge.component.css'
})
export class IntegrityBadgeComponent {
  @Input() isVerified: boolean = true;
}
