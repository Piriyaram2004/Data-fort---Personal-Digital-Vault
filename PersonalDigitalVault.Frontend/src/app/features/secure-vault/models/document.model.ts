export interface DocumentItem {
  id: string;
  fileName: string;
  fileType: string;
  fileSizeBytes: number;
  folderId?: string;
  folderName?: string;
  sha256Hash: string;
  integrityVerified: boolean;
  uploadedAt: string;
}

export interface UploadDocumentRequest {
  file: File;
  folderId?: string;
  description?: string;
}
