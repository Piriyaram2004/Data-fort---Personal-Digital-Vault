import { Component, OnInit, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AdminAuditLogService } from '../../services/admin-audit-log.service';
import { AuditLog } from '../../models/audit-log.model';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-audit-logs',
  standalone: true,
  imports: [DatePipe, LoadingComponent, EmptyStateComponent],
  templateUrl: './audit-logs.component.html',
  styleUrl: './audit-logs.component.css'
})
export class AuditLogsComponent implements OnInit {
  private auditLogService = inject(AdminAuditLogService);

  logs: AuditLog[] = [];
  isLoading = true;

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.isLoading = true;
    this.auditLogService.getAuditLogs().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.logs = res.data;
        }
      },
      error: () => {
        this.isLoading = false;
        this.logs = [];
      }
    });
  }
}
