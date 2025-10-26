import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.scss']
})
export class PedidosComponent implements OnInit {
  pedidos = [
    {
      numero: 'INV-2025-1042',
      fecha: '12 mayo 2025',
      estado: 'En preparación',
      total: 129990,
      items: [
        { nombre: 'Teclado mecánico minimal', cantidad: 1, subtotal: 69990 },
        { nombre: 'Mouse inalámbrico graphite', cantidad: 1, subtotal: 60000 }
      ],
      tracking: 'SF-123456'
    },
    {
      numero: 'INV-2025-0978',
      fecha: '28 abril 2025',
      estado: 'Entregado',
      total: 54990,
      items: [
        { nombre: 'Audífonos Bluetooth blancos', cantidad: 1, subtotal: 54990 }
      ],
      tracking: 'SF-123012'
    }
  ];

  direcciones = [
    {
      alias: 'Casa',
      nombre: 'María López',
      direccion: 'Los Robles 1234, Departamento 502',
      ciudad: 'Providencia, Santiago',
      telefono: '+56 9 4444 5555',
      principal: true
    },
    {
      alias: 'Oficina',
      nombre: 'María López',
      direccion: 'Av. Apoquindo 3120, Piso 8',
      ciudad: 'Las Condes, Santiago',
      telefono: '+56 2 2222 3333',
      principal: false
    }
  ];

  direccionForm = this.fb.group({
    alias: ['', [Validators.required, Validators.minLength(3)]],
    nombre: ['', [Validators.required, Validators.minLength(3)]],
    direccion: ['', [Validators.required, Validators.minLength(6)]],
    ciudad: ['', [Validators.required]],
    telefono: ['', [Validators.pattern(/^[0-9+\-\s]*$/)]]
  });

  guardado = false;

  constructor(private fb: FormBuilder) {
    this.direccionForm.valueChanges.subscribe(() => (this.guardado = false));
  }

  ngOnInit(): void {
  }

  get totalInvertido(): number {
    return this.pedidos.reduce((acc, pedido) => acc + pedido.total, 0);
  }

  crearDireccion(): void {
    if (this.direccionForm.invalid) {
      this.direccionForm.markAllAsTouched();
      return;
    }

    this.direcciones.forEach(dir => (dir.principal = false));
    this.direcciones.unshift({
      ...this.direccionForm.value,
      principal: true
    } as any);

    this.guardado = true;
    this.direccionForm.reset();
  }

  establecerPrincipal(direccion: any): void {
    this.direcciones.forEach(dir => (dir.principal = dir === direccion));
  }

  estadoChip(estado: string): 'accent' | 'primary' | 'warn' {
    switch (estado) {
      case 'En preparación':
        return 'accent';
      case 'Entregado':
        return 'primary';
      default:
        return 'warn';
    }
  }
}
