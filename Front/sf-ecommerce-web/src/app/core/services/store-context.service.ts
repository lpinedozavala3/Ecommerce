import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, finalize, shareReplay, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TenantInfo } from '../models/tenantInfo';

@Injectable({ providedIn: 'root' })
export class StoreContextService {
  private currentStoreName: string | null = null;
  private pendingStoreName: string | null = null;
  private pendingRequest$: Observable<TenantInfo> | null = null;
  private lastFailedStoreName: string | null = null;
  private readonly storeInfoSubject = new BehaviorSubject<TenantInfo | null>(null);

  constructor(private http: HttpClient) {}

  get storeInfo$(): Observable<TenantInfo | null> {
    return this.storeInfoSubject.asObservable();
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
      return this.pendingRequest$;
    }

    this.currentStoreName = sanitized;
    this.pendingStoreName = normalized;

    const request$ = this.http
      .get<TenantInfo>(`${environment.backend_server}/tienda/${encodeURIComponent(sanitized)}`)
      .pipe(
        tap(info => {
          this.lastFailedStoreName = null;
          this.currentStoreName = info?.nombreFantasia ?? sanitized;
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
        }),
        shareReplay({ bufferSize: 1, refCount: false })
      );

    this.pendingRequest$ = request$;

    return request$;
  }

  clearStore(options?: { preserveFailure?: boolean }): void {
    this.currentStoreName = null;
    this.pendingRequest$ = null;
    this.pendingStoreName = null;
    this.storeInfoSubject.next(null);

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
