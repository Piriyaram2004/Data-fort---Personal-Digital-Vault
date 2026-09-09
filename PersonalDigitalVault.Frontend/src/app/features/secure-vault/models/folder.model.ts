export interface Folder {
  folderId: number;
  userId: number;
  parentFolderId: number | null;
  folderName: string;
  description: string | null;
  isDeleted: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateFolderRequest {
  folderName: string;
  parentFolderId: number | null;
  description: string | null;
}

export interface UpdateFolderRequest {
  folderName: string;
  description: string | null;
}