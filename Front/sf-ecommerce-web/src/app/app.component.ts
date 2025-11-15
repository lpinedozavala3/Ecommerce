import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NavigationStart, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { filter, take } from 'rxjs/operators';
import { StoreContextService } from './core/services/store-context.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'sf-ecommerce-web';
  private subscription: Subscription | null = null;
  private routerSubscription: Subscription | null = null;
  private primeStoreSubscription: Subscription | null = null;
  storeLoading$: Observable<boolean> = this.storeContext.storeLoading$;

  constructor(private titleService: Title, private storeContext: StoreContextService, private router: Router) {}

  ngOnInit(): void {
    this.subscription = this.storeContext.storeInfo$.subscribe(info => {
      if (info && info.nombreFantasia) {
        this.titleService.setTitle(info.nombreFantasia);
      } else {
        this.titleService.setTitle('Mi Tienda');
      }
    });

    this.primeStoreValidation(this.getInitialUrl());

    this.routerSubscription = this.router.events
      .pipe(filter(event => event instanceof NavigationStart))
      .subscribe(event => {
        const navigation = event as NavigationStart;
        this.primeStoreValidation(navigation.url);
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
    if (this.primeStoreSubscription) {
      this.primeStoreSubscription.unsubscribe();
    }
  }

  private getInitialUrl(): string {
    if (this.router.url && this.router.url !== '/') {
      return this.router.url;
    }

    if (typeof window !== 'undefined' && window.location?.pathname) {
      return window.location.pathname;
    }

    return '';
  }

  private primeStoreValidation(url: string | null | undefined): void {
    const candidate = this.extractStoreFromUrl(url);
    if (!candidate) {
      return;
    }

    const active = this.storeContext.getActiveStoreName();
    if (active && active.toLowerCase() === candidate.toLowerCase()) {
      return;
    }

    this.primeStoreSubscription?.unsubscribe();
    this.primeStoreSubscription = this.storeContext
      .ensureStore(candidate)
      .pipe(take(1))
      .subscribe({
        next: () => {},
        error: () => {}
      });
  }

  private extractStoreFromUrl(url?: string | null): string | null {
    if (!url) {
      return null;
    }

    const cleanUrl = url.split('#')[0]?.split('?')[0] ?? '';
    const segments = cleanUrl.split('/').filter(segment => !!segment);
    if (!segments.length) {
      return null;
    }

    const candidate = decodeURIComponent(segments[0].split(';')[0] ?? '');
    if (!candidate || candidate === 'pagina-no-encontrada') {
      return null;
    }

    return candidate;
  }
}
