export interface TenantInfo {
  tiendaId: string;
  emisorId: string;
  nombreComercial: string;
  nombreFantasia: string;
  plantilla: string | null;
  brandingJson?: string | null;
}
