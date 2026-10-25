import { inject, Injectable } from '@angular/core';
import { ApiService } from '../http-client/api.service';
import { StorageService } from '../storage/storage.service';
import { Router } from '@angular/router';
import { SignalrService } from '../signalR/signalr.service';
import { finalize, firstValueFrom, from, switchMap } from 'rxjs';
import { session } from '../../entities/session.entities';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {

   public api = inject(ApiService);
   public session = inject(StorageService);
   public router = inject(Router);
   private signalR = inject(SignalrService);
   
  constructor() { }


  public  logoutService() {
    this.logoutSignalrService().subscribe({
      error: () => {
        console.error("Error al cerrar sesión en SignalR.");
      }
    });
  }

private logoutSignalrService() {
  return from(this.session.get("dta_session") as Promise<session>).pipe(
    switchMap(({ identification }) => 
      this.api.getById(`User/force-logout-signalr`, `id=${identification}`)
    )
  );
}
  public logoutSessionComplete() {
     this.api.getAll(`User/logout-session`).
     subscribe({
      next: () => this.finishLogout(),
      error: () => this.finishLogout()
    });
  }

  /** Corta SignalR, limpia la sesión local y vuelve al login (también si el backend no responde). */
  private async finishLogout() {
    await this.signalR.stopConnection();
    this.session.clear();
    this.router.navigateByUrl("/login");
  }
}
