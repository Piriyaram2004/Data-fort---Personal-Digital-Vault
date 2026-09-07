import { Component, OnInit, inject } from '@angular/core';
import { AdminUserService } from '../../services/admin-user.service';
import { AdminUser } from '../../models/admin-user.model';
import { UserTableComponent } from '../../components/user-table/user-table.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    UserTableComponent,
    LoadingComponent,
    EmptyStateComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  private adminUserService = inject(AdminUserService);

  users: AdminUser[] = [];
  isLoading = true;

  showDeleteDialog = false;
  selectedUserId: string | null = null;

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.adminUserService.getUsers().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.users = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
        this.users = [];
      }
    });
  }

  onToggleUserStatus(user: AdminUser): void {
    this.adminUserService.updateUser(user.id, { role: user.role, isActive: !user.isActive }).subscribe({
      next: () => {
        this.loadUsers();
      }
    });
  }

  onPromptDeleteUser(id: string): void {
    this.selectedUserId = id;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {
    if (!this.selectedUserId) return;
    this.adminUserService.deleteUser(this.selectedUserId).subscribe({
      next: () => {
        this.showDeleteDialog = false;
        this.loadUsers();
      },
      error: () => {
        this.showDeleteDialog = false;
      }
    });
  }
}
