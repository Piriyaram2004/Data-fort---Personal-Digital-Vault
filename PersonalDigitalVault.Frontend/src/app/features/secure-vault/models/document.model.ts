export interface DocumentItem {
  documentId: number;
  userId: number;
  folderId: number | null;
  originalFileName: string;
  fileType: string;
  fileSize: number;
  sha256Hash: string;
  isEncrypted: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface UploadDocumentRequest {
  file: File;
  folderId: number | null;
}