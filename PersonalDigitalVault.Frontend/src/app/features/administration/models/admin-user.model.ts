export interface AdminUser {
  userId: number;
  email: string;
  userName: string;
  fullName: string;
  profileImageUrl: string | null;
  isActive: boolean;
  createdAt: string;
  roleName: string;
}

export interface UpdateUserStatusRequest {
  isActive: boolean;
}