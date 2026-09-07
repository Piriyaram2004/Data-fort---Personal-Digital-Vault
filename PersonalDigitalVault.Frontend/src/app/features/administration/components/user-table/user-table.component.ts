import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AdminUser } from '../../models/admin-user.model';

@Component({
  selector: 'app-user-table',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './user-table.component.html',
  styleUrl: './user-table.component.css'
})
export class UserTableComponent {
  @Input() users: AdminUser[] = [];
  @Output() toggleStatus = new EventEmitter<AdminUser>();
  @Output() deleteUser = new EventEmitter<string>();

  onToggle(user: AdminUser): void {
    this.toggleStatus.emit(user);
  }

  onDelete(id: string): void {
    this.deleteUser.emit(id);
  }
}
