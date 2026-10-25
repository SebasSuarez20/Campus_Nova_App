import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Router } from '@angular/router';
import { StorageService } from '../storage/storage.service';
import { ApiService } from '../http-client/api.service';


@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));
  private hubConnection?: signalR.HubConnection;
  public  listeningObservable$ = new Subject<void>();
  private router = inject(Router);
  private storage = inject(StorageService);
  private api = inject(ApiService);

  constructor() { }


  public startConnection(isRetry = false):Promise<void>{
    if (!this.isBrowser) {
      return Promise.resolve();
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(
        environment.signalrUrl, { withCredentials: true }
      )
      .withAutomaticReconnect()
      .build();

       return this.hubConnection.start()
      .then(() => {
        console.log('SignalR conectado');
        this.listenToNotifications();
      })
      .catch(err => {
        console.error('Error SignalR', err);
        const errMsg = err?.toString() || '';
        if (errMsg.includes('401') || errMsg.includes('Unauthorized')) {
          if (!isRetry) {
            console.log('SignalR falló con 401. Intentando refrescar la sesión...');
            this.api.validatorSession().subscribe({
              next: () => {
                console.log('Sesión validada/refrescada con éxito. Reintentando SignalR...');
                this.startConnection(true);
              },
              error: (refreshErr) => {
                console.error('Fallo en la validación de sesión tras error de SignalR:', refreshErr);
                this.storage.clear();
                this.router.navigate(['/login']);
              }
            });
          } else {
            this.storage.clear();
            this.router.navigate(['/login']);
          }
        }
      });
  }

  public listenToNotifications(): void {
    if (!this.isBrowser) {
      return;
    }

    this.hubConnection?.on(
      'ReceiveNotification',
      async (message: string) => {
        this.listeningObservable$.next();
      });
  }

  public async stopConnection(): Promise<void> {
    if (!this.isBrowser) {
      return;
    }
    if (!this.hubConnection) {
      return;
    }

    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      console.log("Sigue conectado..pero ya lo voy a desconectar");
      await this.hubConnection.stop();
    }
  }
}
