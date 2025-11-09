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
    if (!storeName) {
      this.storeContext.clearStore();
      return of(this.router.parseUrl('/pagina-no-encontrada'));
    }

    return this.storeContext.ensureStore(storeName).pipe(
      map(() => true),
      catchError(() => {
        this.storeContext.clearStore();
        return of(this.router.parseUrl('/pagina-no-encontrada'));
      })
    );
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    return this.canActivate(childRoute, state);
  }
}
