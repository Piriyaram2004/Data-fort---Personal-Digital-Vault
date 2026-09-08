import { Component, OnInit, inject } from '@angular/core';

import { AdminUserService } from '../../services/admin-user.service';
import {
  AdminUser,
  UpdateUserStatusRequest
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
      },
      error: () => {
        this.users = [];
        this.isLoading = false;
        this.errorMessage = 'Unable to load users.';
      }
    });
  }

  onToggleUserStatus(user: AdminUser): void {
    const request: UpdateUserStatusRequest = {
      isActive: !user.isActive
    };

    this.updatingUserId = user.userId;
    this.errorMessage = '';
    this.successMessage = '';

    this.adminUserService
      .updateUserStatus(user.userId, request)
      .subscribe({
        next: (response: { message: string }) => {
          user.isActive = request.isActive;
          this.successMessage = response.message;
          this.updatingUserId = null;
        },
        error: () => {
          this.errorMessage = 'Unable to update user status.';
          this.updatingUserId = null;
        }
      });
  }
}