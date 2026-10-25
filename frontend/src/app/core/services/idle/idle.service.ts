import { inject, Injectable, NgZone, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { fromEvent, merge, Observable, Subscription, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { LogoutService } from '../logout-session/logout.service';
import Swal from 'sweetalert2';
import { ListeningAppService } from '../listening-app/listening-app.service';

@Injectable({
  providedIn: 'root'
})
export class IdleService {
  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));
  private logoutUser = inject(LogoutService);
  private router = inject(Router);
  private ngZone = inject(NgZone);
  private listeningApp = inject(ListeningAppService);

  // 10 minuto de inactividad
  private readonly idleTimeout = 10 * 60 * 1000;
  private readonly countdownTimeout = 60 * 1000;

//  private readonly idleTimeout = 5 * 1000;      // 10 segundos
// private readonly countdownTimeout = 5 * 1000; // espera 10 segundos


  private activitySubscription?: Subscription;
  private isWarningActive = false;

  constructor() {}


  public startWatching(): void {
    if (!this.isBrowser) return;

    this.stopWatching();

    const activityEvents$ = merge(
      fromEvent(window, 'mousemove'),
      fromEvent(window, 'keydown'),
      fromEvent(window, 'click'),
      fromEvent(window, 'scroll'),
      fromEvent(window, 'touchstart')
    );

    this.ngZone.runOutsideAngular(() => {
      this.activitySubscription = activityEvents$
        .pipe(
          switchMap(() => {
            if (this.isWarningActive) {
              return new Observable<never>();
            }
            return timer(this.idleTimeout);
          })
        )
        .subscribe(() => {
          // Expiró el tiempo de inactividad de 9 minutos. Volvemos al hilo de Angular para renderizar la alerta
          this.ngZone.run(() => {
            this.showInactivityWarning();
          });
        });
    });
  }

  /**
   * Detiene el monitoreo y cierra cualquier alerta abierta.
   */
  public stopWatching(): void {
    if (this.activitySubscription) {
      this.activitySubscription.unsubscribe();
      this.activitySubscription = undefined;
    }
    this.closeWarning();
  }

  /**
   * Muestra un modal SweetAlert2 con una cuenta regresiva dinámica en segundos.
   */
  private showInactivityWarning(): void {
    this.isWarningActive = true;
    let timerInterval: any;

    Swal.fire({
      title: 'Sesión Inactiva',
      html: 'Tu sesión se cerrará automáticamente en <b>60</b> segundos por inactividad.',
      timer: this.countdownTimeout,
      timerProgressBar: true,
      icon: 'warning',
      showCancelButton: false,
      confirmButtonColor: '#F5C518',
      confirmButtonText: 'Seguir trabajando',
      allowOutsideClick: false,
      allowEscapeKey: false,
      didOpen: () => {
        const timerHtml = Swal.getHtmlContainer()?.querySelector('b');
        timerInterval = setInterval(() => {
          if (timerHtml) {
            const timeLeft = Math.ceil((Swal.getTimerLeft() || 0) / 1000);
            timerHtml.textContent = timeLeft.toString();
          }
        }, 1000);
      },
      willClose: () => {
        clearInterval(timerInterval);
      }
    }).then((result) => {
      this.isWarningActive = false;
      if (result.isConfirmed) {
        this.startWatching();
      } else if (result.dismiss === Swal.DismissReason.timer) {
        this.logoutUser.logoutSessionComplete();
        this.listeningApp.setListeningObservable("Sesión cerrada por inactividad. Por favor, inicia sesión nuevamente para continuar trabajando.");
      }
    });
  }

  private closeWarning(): void {
    if (this.isWarningActive) {
      Swal.close();
      this.isWarningActive = false;
    }
  }
}
