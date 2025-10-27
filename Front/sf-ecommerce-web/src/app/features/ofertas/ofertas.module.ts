import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { OfertasRoutingModule } from './ofertas-routing.module';
import { OfertasComponent } from './ofertas.component';

@NgModule({
  declarations: [OfertasComponent],
  imports: [CommonModule, OfertasRoutingModule, MatCardModule, MatButtonModule, MatIconModule]
})
export class OfertasModule {}
