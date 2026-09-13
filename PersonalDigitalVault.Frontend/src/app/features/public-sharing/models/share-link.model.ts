export interface ShareLink {
  shareLinkId: number;
  documentId: number;
  shareToken: string;
  expiresAt: string | null;
  isRevoked: boolean;
  createdAt: string;
}

export interface CreateShareLinkRequest {
  documentId: number;
  expiresAt: string | null;
}

export interface PublicFileDetails {
  fileName: string;
  fileType: string;
  fileSize: number;
  expiresAt: string | null;
}