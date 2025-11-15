import { Component, OnInit } from '@angular/core';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
  storeName: string = 'Mi Tienda';

  constructor(public store: StoreContextService) { }

  ngOnInit(): void {
    this.store.storeInfo$.subscribe(info => {
      if (info) {
        this.storeName = info.nombreFantasia || 'Mi Tienda';
      }
    });
  }

}
