import { Routes } from '@angular/router';
import { ShareLinkComponent } from './pages/share-link/share-link.component';
import { PublicFileComponent } from './pages/public-file/public-file.component';

export const PUBLIC_SHARING_ROUTES: Routes = [
  { path: 'manage', component: ShareLinkComponent },
  { path: 'file/:token', component: PublicFileComponent },
  { path: '', redirectTo: 'manage', pathMatch: 'full' }
];
