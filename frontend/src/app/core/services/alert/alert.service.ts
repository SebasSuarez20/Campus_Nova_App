import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import Swal, { SweetAlertResult } from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AlertService {


public success(title: string, message: string = ''): Promise<SweetAlertResult> {
   return Swal.fire({
      icon: 'success',
      title,
      text: message,
      showConfirmButton:false,
      timer: 2500,
      timerProgressBar: true,
      toast: true
    });
  }

public error(title: string, message: string = ''): Promise<SweetAlertResult> {
   return Swal.fire({
      icon: 'error',
      title,
      text: message,
      showConfirmButton:false,
      timer: 2500,
      timerProgressBar: true,
      toast: true
    });
  }


public warning(title: string, message: string = ''): Promise<SweetAlertResult> {
   return Swal.fire({
      icon: 'warning',
      title,
      text: message,
      showConfirmButton:false,
      timer: 2500,
      timerProgressBar: true,
      toast: true
    });
  }


public info(title: string, message: string = ''): Promise<SweetAlertResult> {
   return Swal.fire({
      icon: 'info',
      title,
      text: message,
      showConfirmButton:false,
      timer: 2500,
      timerProgressBar: true,
      toast: true
    });
  }

  // 🗑️ Confirmación (para eliminar o acciones críticas)
public  confirm(title: string, message: string = ''): Promise<SweetAlertResult> {
    return Swal.fire({
        icon: 'warning',
        title,
        text: message,
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sí, continuar',
        cancelButtonText: 'Cancelar',
        toast:true
      })
  }

  // ⏳ Loading (mostrar mientras espera)
  public loading(title: string = 'Cargando...'): Promise<SweetAlertResult> {
   return Swal.fire({
      title,
      allowOutsideClick: false,
      showConfirmButton:false,
      timer: 2500,
      timerProgressBar: true,
      toast: true,
      didOpen: () => Swal.showLoading()
    });
  }

  public errorWithHtml(body: string[]) {

    Swal.fire({
      icon: 'error',
      title: '¡Campos inválidos!',
      html: `
        <p style="color:#888; font-size:0.8rem; margin-bottom:0.5rem">Se encontraron ${body.length} error(es):</p>
        <hr style="border:none; border-top:1px solid #f0f0f0; margin-bottom:0.8rem">
        <ul style="text-align:left; margin:0; padding:0; list-style:none; color:#555; font-size:0.9rem; line-height:2">
          ${body.map(e => `<li>❌ ${e}</li>`).join('')}
        </ul>
        <hr style="border:none; border-top:1px solid #f0f0f0; margin-top:0.8rem">
      `,
      toast: true,
      showCancelButton: false,
      confirmButtonText: 'Entendido',
      confirmButtonColor: '#d33'
    })
  }

  // Cerrar cualquier alerta activa
  public close(): void {
    Swal.close();
  }
}
