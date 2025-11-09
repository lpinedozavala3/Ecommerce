import { Component, OnInit } from '@angular/core';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss']
})
export class NotFoundComponent implements OnInit {
  constructor(private storeContext: StoreContextService) {}

  ngOnInit(): void {
    this.storeContext.clearStore();
  }
}
