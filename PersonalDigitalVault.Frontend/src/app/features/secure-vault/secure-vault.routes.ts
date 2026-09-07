import { Routes } from '@angular/router';
import { VaultHomeComponent } from './pages/vault-home/vault-home.component';
import { FolderListComponent } from './pages/folders/folder-list/folder-list.component';
import { FolderFormComponent } from './pages/folders/folder-form/folder-form.component';
import { DocumentListComponent } from './pages/documents/document-list/document-list.component';
import { DocumentUploadComponent } from './pages/documents/document-upload/document-upload.component';
import { DocumentDetailsComponent } from './pages/documents/document-details/document-details.component';
import { CredentialListComponent } from './pages/credentials/credential-list/credential-list.component';
import { CredentialFormComponent } from './pages/credentials/credential-form/credential-form.component';
import { SearchComponent } from './pages/search/search.component';

export const SECURE_VAULT_ROUTES: Routes = [
  { path: '', component: VaultHomeComponent },
  { path: 'folders', component: FolderListComponent },
  { path: 'folders/new', component: FolderFormComponent },
  { path: 'folders/edit/:id', component: FolderFormComponent },
  { path: 'documents', component: DocumentListComponent },
  { path: 'documents/upload', component: DocumentUploadComponent },
  { path: 'documents/:id', component: DocumentDetailsComponent },
  { path: 'credentials', component: CredentialListComponent },
  { path: 'credentials/new', component: CredentialFormComponent },
  { path: 'credentials/edit/:id', component: CredentialFormComponent },
  { path: 'search', component: SearchComponent }
];
