export interface CredentialItem {
  credentialId: number;
  userId: number;
  folderId: number | null;
  title: string;
  userName: string;
  password: string;
  notes: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCredentialRequest {
  folderId: number | null;
  title: string;
  userName: string;
  password: string;
  notes: string | null;
}

export interface UpdateCredentialRequest {
  folderId: number | null;
  title: string;
  userName: string;
  password: string;
  notes: string | null;
}