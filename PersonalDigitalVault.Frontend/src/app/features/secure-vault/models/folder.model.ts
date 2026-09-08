export interface Folder {
  id: string;
  name: string;
  description?: string;
  documentCount?: number;
  createdAt: string;
}

export interface CreateFolderRequest {
  name: string;
  description?: string;
}
