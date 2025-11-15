import { Component } from '@angular/core';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-ofertas',
  templateUrl: './ofertas.component.html',
  styleUrls: ['./ofertas.component.scss']
})
export class OfertasComponent {
  constructor(public store: StoreContextService) {}
  destacados = [
    {
      titulo: 'Bundles esenciales para tu escritorio',
      descripcion: 'Mouse, teclado y audífonos con 25% de descuento al comprar el kit completo.',
      ahorro: 'Ahorra hasta $35.000',
      etiqueta: 'Pack destacado',
      tema: 'sunset'
    },
    {
      titulo: 'Lanzamiento + preventa',
      descripcion: 'Reserva anticipada de lanzamientos y recibe un cupón de 15% en tu próxima compra.',
      ahorro: 'Cupón de regalo',
      etiqueta: 'Preventa',
      tema: 'ocean'
    },
    {
      titulo: 'Ofertas sustentables',
      descripcion: 'Productos reacondicionados con garantía extendida por 12 meses.',
      ahorro: 'Envío gratis incluido',
      etiqueta: 'Eco selección',
      tema: 'forest'
    }
  ];

  beneficios = [
    {
      icono: 'local_shipping',
      titulo: 'Despacho express',
      descripcion: 'Envíos en 24 horas en Santiago y 48 horas en regiones seleccionadas.'
    },
    {
      icono: 'autorenew',
      titulo: 'Cambios sin costo',
      descripcion: 'Tienes 30 días para cambios o devoluciones en compras online.'
    },
    {
      icono: 'verified_user',
      titulo: 'Garantía extendida',
      descripcion: 'Hasta 12 meses extra en categorías seleccionadas.'
    }
  ];

  cupones = [
    { codigo: 'BIENVENIDO10', descripcion: '10% de descuento en tu primera compra', vigencia: 'Válido hasta 30/09' },
    { codigo: 'FREESHIP', descripcion: 'Despacho gratis sobre $49.990', vigencia: 'Durante todo el mes' },
    { codigo: 'PLUS20', descripcion: '20% off en accesorios seleccionados', vigencia: 'Solo fin de semana' }
  ];
}
