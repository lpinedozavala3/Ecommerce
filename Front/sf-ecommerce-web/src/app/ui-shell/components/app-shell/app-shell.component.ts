import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-app-shell',
  templateUrl: './app-shell.component.html',
  styleUrls: ['./app-shell.component.scss']
})
export class AppShellComponent {
  storeLoading$: Observable<boolean> = this.storeContext.storeLoading$;

  constructor(private storeContext: StoreContextService) {}
}
