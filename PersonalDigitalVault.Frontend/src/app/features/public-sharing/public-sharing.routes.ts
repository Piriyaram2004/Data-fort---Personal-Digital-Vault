import { Routes } from '@angular/router';

import { ShareLinkComponent } from './pages/share-link/share-link.component';

export const PUBLIC_SHARING_ROUTES: Routes = [
  { path: 'manage', component: ShareLinkComponent },

  { path: '', redirectTo: 'manage', pathMatch: 'full' }
];