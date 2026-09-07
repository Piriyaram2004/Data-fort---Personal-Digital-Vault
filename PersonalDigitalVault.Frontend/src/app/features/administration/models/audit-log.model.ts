export interface AuditLog {
  id: string;
  userId: string;
  userEmail: string;
  action: string;
  details?: string;
  ipAddress: string;
  timestamp: string;
}
