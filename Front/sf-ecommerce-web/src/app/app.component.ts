import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { StoreContextService } from './core/services/store-context.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'sf-ecommerce-web';
  private subscription: Subscription | null = null;

  constructor(private titleService: Title, private storeContext: StoreContextService) {}

  ngOnInit(): void {
    this.subscription = this.storeContext.storeInfo$.subscribe(info => {
      if (info && info.nombreFantasia) {
        this.titleService.setTitle(info.nombreFantasia);
      } else {
        this.titleService.setTitle('Mi Tienda');
      }
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
