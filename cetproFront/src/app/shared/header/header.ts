import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { AuthService } from '../../core/services/auth.service';
import { IDIOMA_GUARDADO_KEY, IdiomaDisponible } from '../../core/services/idioma.util';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    FormsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatChipsModule,
    MatFormFieldModule,
    MatSelectModule,
    TranslatePipe
  ],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly translateService = inject(TranslateService);

  readonly usuario = this.authService.usuarioActual;

  get idiomaActual(): IdiomaDisponible {
    return (this.translateService.currentLang() as IdiomaDisponible) ?? 'es';
  }

  cambiarIdioma(idioma: IdiomaDisponible): void {
    this.translateService.use(idioma);
    localStorage.setItem(IDIOMA_GUARDADO_KEY, idioma);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }
}
