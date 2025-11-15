import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, finalize, map, shareReplay, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TenantInfo } from '../models/tenantInfo';
import { ApiResponse } from '../models/api-response';

@Injectable({ providedIn: 'root' })
export class StoreContextService {
  private currentStoreName: string | null = null;
  private pendingStoreName: string | null = null;
  private pendingRequest$: Observable<TenantInfo> | null = null;
  private lastFailedStoreName: string | null = null;
  private readonly storeInfoSubject = new BehaviorSubject<TenantInfo | null>(null);
  private readonly storeLoadingSubject = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) {}

  get storeInfo$(): Observable<TenantInfo | null> {
    return this.storeInfoSubject.asObservable();
  }

  get storeLoading$(): Observable<boolean> {
    return this.storeLoadingSubject.asObservable();
  }

  getActiveStoreName(): string | null {
    return this.currentStoreName;
  }

  ensureStore(storeName: string): Observable<TenantInfo> {
    const sanitized = (storeName ?? '').trim();
    if (!sanitized) {
      this.clearStore();
      return throwError(() => new Error('El nombre de fantasía es obligatorio.'));
    }

    const normalized = sanitized.toLowerCase();

    if (this.lastFailedStoreName === normalized) {
      return throwError(() => new Error('La tienda no existe.'));
    }

    if (this.currentStoreName && this.currentStoreName.toLowerCase() === normalized) {
      const cached = this.storeInfoSubject.value;
      if (cached) {
        return of(cached);
      }
    }

    if (this.pendingRequest$ && this.pendingStoreName === normalized) {
      // Asegurarnos de que el tipo devuelto sea Observable<TenantInfo>
      return this.pendingRequest$.pipe(
        map(tenant => {
          if (!tenant) throw new Error('La tienda no existe.');
          return tenant;
        })
      );
    }

    // No asignar currentStoreName hasta confirmar que la tienda existe
    this.pendingStoreName = normalized;
    this.storeLoadingSubject.next(true);

    const request$ = this.http
      .get<ApiResponse<TenantInfo>>(`${environment.backend_server}/tienda/${encodeURIComponent(sanitized)}`)
      .pipe(
        map((resp: ApiResponse<TenantInfo>) => resp.data),
        map((info: TenantInfo) => {
          if (!info || !info.tiendaId) {
            throw new Error('Tienda no encontrada');
          }
          return info;
        }),
        tap((info: TenantInfo) => {
          this.lastFailedStoreName = null;
          // Usar nombreFantasia del backend (con mayúsculas correctas)
          this.currentStoreName = info.nombreFantasia || sanitized;
          this.storeInfoSubject.next(info);
        }),
        catchError(error => {
          this.lastFailedStoreName = normalized;
          this.currentStoreName = null;
          this.storeInfoSubject.next(null);
          return throwError(() => error);
        }),
        finalize(() => {
          this.pendingRequest$ = null;
          this.pendingStoreName = null;
          this.storeLoadingSubject.next(false);
        }),
        shareReplay({ bufferSize: 1, refCount: false })
      );

    // Aseguramos que request$ sea Observable<TenantInfo>
    this.pendingRequest$ = request$.pipe(
      map(tenant => {
        if (!tenant) throw new Error('La tienda no existe.');
        return tenant;
      })
    );

    return request$;
  }

  clearStore(options?: { preserveFailure?: boolean }): void {
    this.currentStoreName = null;
    this.pendingRequest$ = null;
    this.pendingStoreName = null;
    this.storeInfoSubject.next(null);
    this.storeLoadingSubject.next(false);

    if (!options?.preserveFailure) {
      this.lastFailedStoreName = null;
    }
  }

  storeLink(...segments: any[]): any[] {
    const commands = segments.length === 1 && Array.isArray(segments[0]) ? segments[0] : segments;
    const filtered = commands.filter(segment => segment !== undefined && segment !== null);

    if (this.currentStoreName) {
      return ['/', this.currentStoreName, ...filtered];
    }

    return ['/', ...filtered];
  }
}
