import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TenantInfo } from '../models/tenantInfo';

@Injectable({ providedIn: 'root' })
export class StoreContextService {
  private currentStoreName: string | null = null;
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

    if (this.currentStoreName && this.currentStoreName.toLowerCase() === sanitized.toLowerCase()) {
      const cached = this.storeInfoSubject.value;
      if (cached) {
        return of(cached);
      }
    }

    this.currentStoreName = sanitized;

    return this.http
      .get<TenantInfo>(`${environment.backend_server}/tienda/${encodeURIComponent(sanitized)}`)
      .pipe(
        tap(info => {
          this.currentStoreName = info?.nombreFantasia ?? sanitized;
          this.storeInfoSubject.next(info);
        })
      );
  }

  clearStore(): void {
    this.currentStoreName = null;
    this.storeInfoSubject.next(null);
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
