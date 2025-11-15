import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoadingComponent {
  @Input() message?: string;
  @Input() size: 'sm' | 'md' | 'lg' = 'md';
  @Input() inline = false;
  @Input() fullscreen = false;
  @Input() ariaLabel = 'Cargando';

  get sizePx(): number {
    switch (this.size) {
      case 'sm':
        return 18;
      case 'lg':
        return 64;
      default:
        return 32;
    }
  }
}
