import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Header } from '../../../shared/header/header';

@Component({
  selector: 'app-estudiante-layout',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet, Header],
  templateUrl: './estudiante-layout.html',
  styleUrl: './estudiante-layout.css'
})
export class EstudianteLayout {}
