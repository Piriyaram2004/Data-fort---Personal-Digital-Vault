import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { UsersComponent } from './pages/users/users.component';
import { AuditLogsComponent } from './pages/audit-logs/audit-logs.component';

export const ADMINISTRATION_ROUTES: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'users', component: UsersComponent },
  { path: 'audit-logs', component: AuditLogsComponent },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
];
