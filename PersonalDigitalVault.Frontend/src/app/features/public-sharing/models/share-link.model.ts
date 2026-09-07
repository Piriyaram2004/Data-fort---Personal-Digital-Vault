export interface ShareLink {
  id: string;
  documentId: string;
  documentName: string;
  shareToken: string;
  expiryDate: string;
  isRevoked: boolean;
  accessCount: number;
  createdAt: string;
}

export interface CreateShareLinkRequest {
  documentId: string;
  expiryDays: number;
}

export interface PublicFileDetails {
  documentName: string;
  fileSizeBytes: number;
  fileType: string;
  expiryDate: string;
  shareToken: string;
}
