import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';

import { AdminUserService } from '../../services/admin-user.service';
import {
  AdminUser,
  UpdateAdminUserStatusRequest
} from '../../models/admin-user.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  private readonly adminUserService = inject(AdminUserService);
  private readonly cdr = inject(ChangeDetectorRef);

  users: AdminUser[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  updatingUserId: number | null = null;

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminUserService.getUsers().subscribe({
      next: (users: AdminUser[]) => {
        this.users = users;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.users = [];
        this.isLoading = false;
        this.errorMessage = 'Unable to load users.';
        this.cdr.detectChanges();
      }
    });
  }

  onToggleUserStatus(user: AdminUser): void {
    const request: UpdateAdminUserStatusRequest = {
      isActive: !user.isActive
    };

    this.updatingUserId = user.userId;
    this.errorMessage = '';
    this.successMessage = '';

    this.adminUserService
      .updateUserStatus(user.userId, request)
      .subscribe({
        next: () => {
          user.isActive = request.isActive;
          this.successMessage = 'User status updated successfully.';
          this.updatingUserId = null;
          this.cdr.detectChanges();
        },
        error: () => {
          this.errorMessage = 'Unable to update user status.';
          this.updatingUserId = null;
          this.cdr.detectChanges();
        }
      });
  }
}