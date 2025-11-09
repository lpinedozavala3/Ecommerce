import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { StoreContextService } from './services/store-context.service';

@Injectable()
export class TenantInterceptor implements HttpInterceptor {
  constructor(private injector: Injector) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const storeContext = this.injector.get(StoreContextService);
    let storeName = storeContext.getActiveStoreName();

    if (!storeName) {
      const isLocalhost = window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1';
      if (isLocalhost && environment.devTenantName) {
        storeName = environment.devTenantName;
      }
    }

    if (!storeName) {
      return next.handle(req);
    }

    const cloned = req.clone({
      setHeaders: { 'X-Store-Name': storeName }
    });

    return next.handle(cloned);
  }
}
