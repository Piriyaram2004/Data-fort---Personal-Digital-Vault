export interface CredentialItem {
  id: string;
  serviceName: string;
  username: string;
  secretValue?: string; // Decrypted value returned only when explicitly requested from backend
  notes?: string;
  createdAt: string;
}

export interface CreateCredentialRequest {
  serviceName: string;
  username: string;
  secretValue: string;
  notes?: string;
}
