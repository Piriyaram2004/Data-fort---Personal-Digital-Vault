import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { DashboardModel } from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class AdminDashboardService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/admin/dashboard`;

  getDashboard(): Observable<DashboardModel> {
    return this.http.get<DashboardModel>(this.apiUrl);
  }
}