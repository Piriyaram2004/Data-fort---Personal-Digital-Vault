export interface UserProfile {
  userId: number;
  email: string;
  userName: string;
  fullName: string;
  profileImageUrl: string | null;
  role: string;
}

export interface UpdateProfileRequest {
  username: string;
  email: string;
}