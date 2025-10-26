import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

import { AuthStateService } from 'src/app/core/services/auth-state.service';
import { PedidosService } from 'src/app/core/services/pedidos.service';
import { DireccionCliente, PedidoResumen, UpsertDireccionPayload } from 'src/app/core/models/pedidos';
import { UsuarioSesion } from 'src/app/core/models/auth';

@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.scss']
})
export class PedidosComponent implements OnInit, OnDestroy {
  pedidos: PedidoResumen[] = [];
  direccion: DireccionCliente | null = null;
  usuario: UsuarioSesion | null = null;

  cargandoPedidos = false;
  cargandoDireccion = false;
  guardado = false;
  errorPedidos = '';
  errorDireccion = '';

  private suscripcion?: Subscription;

  direccionForm = this.fb.group({
    calle: ['', [Validators.required, Validators.minLength(4)]],
    numero: [''],
    depto: [''],
    comuna: ['', [Validators.required]],
    ciudad: ['', [Validators.required]],
    region: ['', [Validators.required]],
    pais: ['', [Validators.required]],
    codigoPostal: [''],
    referencias: ['']
  });

  constructor(
    private fb: FormBuilder,
    private authState: AuthStateService,
    private pedidosService: PedidosService
  ) {
    this.direccionForm.valueChanges.subscribe(() => (this.guardado = false));
  }

  ngOnInit(): void {
    this.usuario = this.authState.usuario;
    this.suscripcion = this.authState.usuario$.subscribe(usuario => {
      this.usuario = usuario;
      this.pedidos = [];
      this.direccion = null;
      this.errorPedidos = '';
      this.errorDireccion = '';
      this.guardado = false;
      this.cargandoPedidos = false;
      this.cargandoDireccion = false;
      this.direccionForm.reset({
        calle: '',
        numero: '',
        depto: '',
        comuna: '',
        ciudad: '',
        region: '',
        pais: '',
        codigoPostal: '',
        referencias: ''
      });

      if (usuario) {
        this.cargarPedidos(usuario.id);
        this.cargarDireccion(usuario.id);
      }
    });

  }

  ngOnDestroy(): void {
    this.suscripcion?.unsubscribe();
  }

  get totalInvertido(): number {
    return this.pedidos.reduce((acc, pedido) => acc + pedido.total, 0);
  }

  get estaAutenticado(): boolean {
    return !!this.usuario;
  }

  cargarPedidos(clienteId: string): void {
    this.cargandoPedidos = true;
    this.errorPedidos = '';
    this.pedidosService.obtenerPedidos(clienteId).subscribe({
      next: pedidos => {
        this.pedidos = pedidos;
        this.cargandoPedidos = false;
      },
      error: () => {
        this.errorPedidos = 'No pudimos recuperar tus pedidos. Intenta nuevamente en unos minutos.';
        this.cargandoPedidos = false;
      }
    });
  }

  cargarDireccion(clienteId: string): void {
    this.cargandoDireccion = true;
    this.errorDireccion = '';
    this.pedidosService.obtenerDireccion(clienteId).subscribe({
      next: direccion => {
        this.direccion = direccion;
        this.cargandoDireccion = false;

        if (direccion) {
          this.direccionForm.patchValue({
            calle: direccion.calle,
            numero: direccion.numero ?? '',
            depto: direccion.depto ?? '',
            comuna: direccion.comuna,
            ciudad: direccion.ciudad,
            region: direccion.region,
            pais: direccion.pais,
            codigoPostal: direccion.codigoPostal ?? '',
            referencias: direccion.referencias ?? ''
          });
        }
      },
      error: () => {
        this.errorDireccion = 'No pudimos recuperar tu dirección principal.';
        this.cargandoDireccion = false;
      }
    });
  }

  guardarDireccion(): void {
    if (!this.usuario) {
      return;
    }

    if (this.direccionForm.invalid) {
      this.direccionForm.markAllAsTouched();
      return;
    }

    const formValue = this.direccionForm.value;
    const payload: UpsertDireccionPayload = {
      calle: (formValue.calle ?? '').trim(),
      comuna: (formValue.comuna ?? '').trim(),
      ciudad: (formValue.ciudad ?? '').trim(),
      region: (formValue.region ?? '').trim(),
      pais: (formValue.pais ?? '').trim(),
      numero: formValue.numero?.toString().trim() || null,
      depto: formValue.depto?.toString().trim() || null,
      codigoPostal: formValue.codigoPostal?.toString().trim() || null,
      referencias: formValue.referencias?.toString().trim() || null
    };

    this.guardado = false;
    this.cargandoDireccion = true;
    this.pedidosService.guardarDireccion(this.usuario.id, payload).subscribe({
      next: direccion => {
        this.direccion = direccion;
        this.guardado = true;
        this.cargandoDireccion = false;
      },
      error: () => {
        this.errorDireccion = 'No pudimos guardar la dirección. Intenta nuevamente.';
        this.cargandoDireccion = false;
      }
    });
  }

  formatFecha(fechaIso: string): string {
    if (!fechaIso) {
      return '—';
    }

    const fecha = new Date(fechaIso);
    if (Number.isNaN(fecha.getTime())) {
      return '—';
    }

    return fecha.toLocaleDateString('es-CL', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }
}
