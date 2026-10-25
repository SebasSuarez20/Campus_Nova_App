import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import Swal from 'sweetalert2';
import { ListeningAppService } from './core/services/listening-app/listening-app.service';
import { TabDuplicationService } from './core/services/tab-guard/tab-duplication.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: '<router-outlet />'
})
export class AppComponent implements OnInit {

  public title = 'Campus Nova';

  private readonly listeningApp = inject(ListeningAppService);
  private readonly tabDuplicationService = inject(TabDuplicationService);

  public isDuplicate = false;

  ngOnInit(): void {
    this.listeningObservable();
    this.sessionActiveVerify();
    this.checkTabDuplication();
  }

  /** Marca la pestaña como duplicada si otra pestaña responde al mismo tab_id. */
  private checkTabDuplication(): void {
    this.tabDuplicationService.isDuplicate$.subscribe((isDup) => {
      this.isDuplicate = isDup;
    });
  }

  private listeningObservable(): void {
    this.listeningApp.listeningObservable().subscribe((res) => {
      localStorage.setItem("session_inactive", "true");
      this.showAlert(res);
    });
  }

  /** Si la sesión se cerró por inactividad en otra carga, avisa al volver. */
  private sessionActiveVerify(): void {
    const isSessionInactive = localStorage.getItem("session_inactive");
    if (isSessionInactive) {
      Swal.close();
      this.showAlert("Cerramos tu sesión por inactividad. Vuelve a entrar para continuar.");
    }
  }

  private showAlert(message: string): void {
    Swal.fire({
      icon: 'warning',
      toast: true,
      title: message,
      showCancelButton: false,
      showConfirmButton: true
    }).then(() => localStorage.clear());
  }
}
