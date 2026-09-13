export interface RegisterRequest {
  email: string;
  userName: string;
  fullName: string;
  password: string;
  confirmPassword: string;
}

export interface RegisterResponse {
  userId: number;
  email: string;
  userName: string;
  fullName: string;
  role: string;
}