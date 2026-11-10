import { Injectable, signal } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel
} from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Matricula, Pago, PagoAprobadoEvento, EstadoMatricula } from '../models/matricula.models';

const TOKEN_KEY = 'cetpro_token';
const REINTENTO_MS = 5000;

@Injectable({ providedIn: 'root' })
export class NotificacionesService {
  private connection: HubConnection | null = null;
  private reintentoTimer: ReturnType<typeof setTimeout> | null = null;

  private readonly nuevaMatriculaSubject = new Subject<Matricula>();
  private readonly pagoAprobadoSubject = new Subject<PagoAprobadoEvento>();
  private readonly conectadoSignal = signal(false);

  readonly nuevaMatricula$: Observable<Matricula> = this.nuevaMatriculaSubject.asObservable();
  readonly pagoAprobado$: Observable<PagoAprobadoEvento> = this.pagoAprobadoSubject.asObservable();
  readonly conectado = this.conectadoSignal.asReadonly();

  async iniciar(): Promise<void> {
    if (!localStorage.getItem(TOKEN_KEY)) return;
    if (this.connection && this.connection.state !== HubConnectionState.Disconnected) return;

    this.cancelarReintento();
    //web socket
    const connection = new HubConnectionBuilder()//web socket, con jwt 
      .withUrl(environment.hubUrl, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: () => localStorage.getItem(TOKEN_KEY) ?? ''
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    connection.on('NuevaMatricula', (matricula: Matricula) => this.nuevaMatriculaSubject.next(matricula));
    connection.on('PagoAprobado', (pago: Pago, estadoMatricula: EstadoMatricula) =>
      this.pagoAprobadoSubject.next({ pago, estadoMatricula })
    );

    connection.onreconnecting(() => this.conectadoSignal.set(false));
    connection.onreconnected(() => this.conectadoSignal.set(true));
    connection.onclose(() => {
      this.conectadoSignal.set(false);
      // Si se agotaron los reintentos automáticos (y no fue un detener() manual), seguimos intentando.
      if (this.connection === connection) this.programarReintento();
    });

    this.connection = connection;

    try {
      await connection.start();
      this.conectadoSignal.set(true);
    } catch {
      this.conectadoSignal.set(false);
      this.programarReintento();
    }
  }

  async detener(): Promise<void> {
    this.cancelarReintento();
    const connection = this.connection;
    this.connection = null;
    this.conectadoSignal.set(false);

    if (connection) {
      await connection.stop();
    }
  }

  private programarReintento(): void {
    this.reintentoTimer = setTimeout(() => {
      this.reintentoTimer = null;
      void this.iniciar();
    }, REINTENTO_MS);
  }

  private cancelarReintento(): void {
    if (this.reintentoTimer) {
      clearTimeout(this.reintentoTimer);
      this.reintentoTimer = null;
    }
  }
}
