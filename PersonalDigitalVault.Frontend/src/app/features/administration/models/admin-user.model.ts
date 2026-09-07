export interface AdminUser {
  id: string;
  username: string;
  email: string;
  role: string;
  isActive: boolean;
  createdAt: string;
}

export interface UpdateUserRoleRequest {
  role: string;
  isActive: boolean;
}
