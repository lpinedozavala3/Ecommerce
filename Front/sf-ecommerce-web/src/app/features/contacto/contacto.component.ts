import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-contacto',
  templateUrl: './contacto.component.html',
  styleUrls: ['./contacto.component.scss']
})
export class ContactoComponent {
  enviado = false;

  canales = [
    { icono: 'support_agent', titulo: 'Soporte general', descripcion: 'Lunes a viernes de 09:00 a 18:00 hrs.' },
    { icono: 'shopping_bag', titulo: 'Consultas de ventas', descripcion: 'Asesoría personalizada para empresas y personas.' },
    { icono: 'forum', titulo: 'Alianzas comerciales', descripcion: 'Trabajemos juntos para potenciar tu marca.' }
  ];

  contactoForm = this.fb.group({
    nombre: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    telefono: ['', [Validators.pattern(/^[0-9+\-\s]*$/)]],
    motivo: ['soporte', Validators.required],
    mensaje: ['', [Validators.required, Validators.minLength(10)]],
    acepta: [false, Validators.requiredTrue]
  });

  constructor(private fb: FormBuilder) {}

  get controls() {
    return this.contactoForm.controls;
  }

  enviar(): void {
    if (this.contactoForm.invalid) {
      this.contactoForm.markAllAsTouched();
      return;
    }

    this.enviado = true;
    console.table(this.contactoForm.value);
    this.contactoForm.reset({ motivo: 'soporte', acepta: false });
  }
}
