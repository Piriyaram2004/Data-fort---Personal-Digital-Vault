import { Component, OnInit, inject } from '@angular/core';
import { AdminDashboardService } from '../../services/admin-dashboard.service';
import { DashboardStats } from '../../models/dashboard.model';
import { StatCardComponent } from '../../components/stat-card/stat-card.component';
import { FileSizePipe } from '../../../../shared/pipes/file-size.pipe';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [StatCardComponent, FileSizePipe, LoadingComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private adminDashboardService = inject(AdminDashboardService);

  stats: DashboardStats | null = null;
  isLoading = true;

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.isLoading = true;
    this.adminDashboardService.getDashboardStats().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.stats = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
        this.stats = null;
      }
    });
  }
}
