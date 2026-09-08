import { Component, OnInit, inject } from '@angular/core';

import { AdminDashboardService } from '../../services/admin-dashboard.service';
import { DashboardModel } from '../../models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private readonly adminDashboardService = inject(AdminDashboardService);

  dashboard: DashboardModel | null = null;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminDashboardService.getDashboard().subscribe({
      next: (data: DashboardModel) => {
        this.dashboard = data;
        this.isLoading = false;
      },
      error: () => {
        this.dashboard = null;
        this.isLoading = false;
        this.errorMessage = 'Unable to load dashboard information.';
      }
    });
  }
}