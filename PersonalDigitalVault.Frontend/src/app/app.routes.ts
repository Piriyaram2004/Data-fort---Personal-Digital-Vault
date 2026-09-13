import { Routes } from '@angular/router';
import { UserLayoutComponent } from './layouts/user-layout/user-layout.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { PublicFileComponent } from './features/public-sharing/pages/public-file/public-file.component';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  // Unauthenticated Public File Access Route
  { path: 'public/file/:token', component: PublicFileComponent },

  // Module 01: Authentication
  {
    path: 'auth',
    loadChildren: () => import('./features/authentication/authentication.routes').then(m => m.AUTHENTICATION_ROUTES)
  },

  // Module 02 & Module 03 User Features (User Layout + Auth Guard)
  {
    path: '',
    component: UserLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'vault',
        loadChildren: () => import('./features/secure-vault/secure-vault.routes').then(m => m.SECURE_VAULT_ROUTES)
      },
      {
        path: 'sharing',
        loadChildren: () => import('./features/public-sharing/public-sharing.routes').then(m => m.PUBLIC_SHARING_ROUTES)
      },
      { path: '', redirectTo: 'vault', pathMatch: 'full' }
    ]
  },

  // Module 04: Administration (Admin Layout + Auth Guard + Admin Guard)
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [authGuard, adminGuard],
    children: [
      {
        path: '',
        loadChildren: () => import('./features/administration/administration.routes').then(m => m.ADMINISTRATION_ROUTES)
      }
    ]
  },

  // Error Pages
  { path: 'forbidden', component: ForbiddenComponent },
  { path: '**', component: NotFoundComponent }
];

