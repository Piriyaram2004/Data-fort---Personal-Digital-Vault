export interface UserProfile {
  id: string;
  username: string;
  email: string;
  role: string;
  createdAt: string;
}

export interface UpdateProfileRequest {
  username: string;
  email: string;
}
