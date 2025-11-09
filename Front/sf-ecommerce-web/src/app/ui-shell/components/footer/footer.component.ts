import { Component, OnInit } from '@angular/core';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  constructor(public store: StoreContextService) { }

  ngOnInit(): void {
  }

}
