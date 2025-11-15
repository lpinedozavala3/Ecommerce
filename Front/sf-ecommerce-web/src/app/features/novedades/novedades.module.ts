import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { SharedModule } from 'src/app/shared/shared.module';

import { NovedadesRoutingModule } from './novedades-routing.module';
import { NovedadesComponent } from './novedades.component';

@NgModule({
  declarations: [NovedadesComponent],
  imports: [
    CommonModule,
    NovedadesRoutingModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    SharedModule
  ]
})
export class NovedadesModule {}
