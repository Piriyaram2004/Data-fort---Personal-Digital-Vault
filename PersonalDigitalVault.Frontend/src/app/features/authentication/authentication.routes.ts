import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { VerifyEmailComponent } from './pages/verify-email/verify-email.component';


import { authGuard } from '../../core/guards/auth.guard';

export const AUTHENTICATION_ROUTES: Routes = [

  { path: 'login', component: LoginComponent },

  { path: 'register', component: RegisterComponent },

  { path: 'forgot-password', component: ForgotPasswordComponent },

  { path: 'reset-password', component: ResetPasswordComponent },

  { path: 'verify-email', component: VerifyEmailComponent },

  { 
    path: 'change-password', 
    component: ChangePasswordComponent, 
    canActivate: [authGuard] 
  },

  { 
    path: 'profile', 
    component: ProfileComponent, 
    canActivate: [authGuard] 
  },

  { path: '', redirectTo: 'login', pathMatch: 'full' }

];