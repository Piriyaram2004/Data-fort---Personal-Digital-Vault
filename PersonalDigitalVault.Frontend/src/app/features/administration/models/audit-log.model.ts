export interface AuditLog {
  auditLogId: number;
  userId: number | null;
  action: string;
  entityType: string;
  entityId: number | null;
  details: string | null;
  ipAddress: string | null;
  createdAt: string;
}