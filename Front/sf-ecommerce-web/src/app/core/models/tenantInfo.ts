export interface TenantInfo {
  tiendaId: number;
  emisorId: string;
  nombreComercial: string;
  plantilla: string;           // 'T1' | 'T2'...
  brandingJson?: any;
}
