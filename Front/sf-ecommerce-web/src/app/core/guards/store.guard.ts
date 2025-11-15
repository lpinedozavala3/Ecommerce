import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { StoreContextService } from '../services/store-context.service';

@Injectable({ providedIn: 'root' })
export class StoreGuard implements CanActivate, CanActivateChild {
  constructor(private storeContext: StoreContextService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    const storeName = route.paramMap.get('store');
    if (!storeName || !storeName.trim() || storeName === 'pagina-no-encontrada') {
      this.storeContext.clearStore();
      return of(this.router.createUrlTree(['/pagina-no-encontrada']));
    }

    return this.storeContext.ensureStore(storeName).pipe(
      map((tenantInfo) => {
        // Si el nombreFantasia devuelto por el backend es diferente al de la URL, redirigir
        if (tenantInfo.nombreFantasia && tenantInfo.nombreFantasia !== storeName) {
          // Reemplazar el segmento de la tienda en la URL actual
          const urlWithCorrectStore = state.url.replace(`/${storeName}/`, `/${tenantInfo.nombreFantasia}/`);
          return this.router.createUrlTree([urlWithCorrectStore.substring(1)]);
        }
        return true;
      }),
      catchError(() => {
        this.storeContext.clearStore({ preserveFailure: true });
        return of(this.router.createUrlTree(['/pagina-no-encontrada']));
      })
    );
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    return this.canActivate(childRoute, state);
  }
}
