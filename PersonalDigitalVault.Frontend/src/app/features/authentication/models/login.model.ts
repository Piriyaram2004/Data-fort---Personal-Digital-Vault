export interface LoginRequest {
  email: string;
  passwordHash: string; // matches backend property schema or plain password payload
}
