import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NovedadesComponent } from './novedades.component';

const routes: Routes = [{ path: '', component: NovedadesComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NovedadesRoutingModule {}
