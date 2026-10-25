import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AlertService } from '../../core/services/alert/alert.service';
import { RegisterService } from './register.service';

type Campo = 'institutionCode' | 'firstName' | 'lastName' | 'email' | 'identification' | 'password' | 'passwordConfirm';

/**
 * Registro en línea del estudiante. Es una pantalla pública: crea al estudiante
 * y su usuario con POST Student/create-student. Reutiliza los estilos del login.
 */
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['../login/login.component.css', './register.component.css']
})
export class RegisterComponent {

  private readonly service = inject(RegisterService);
  private readonly alert = inject(AlertService);
  private readonly router = inject(Router);

  public readonly form = this.service.createForm();

  public readonly showPassword = signal(false);
  public readonly isLoading = signal(false);
  public readonly intentoEnviar = signal(false);

  public togglePassword(): void {
    this.showPassword.update(v => !v);
  }

  /** El campo se marca como inválido después de tocarlo o de intentar enviar. */
  public invalido(campo: Campo): boolean {
    const control = this.form.controls[campo];
    return control.invalid && (control.touched || this.intentoEnviar());
  }

  /** ¿La contraseña escrita cumple esta regla? (para el checklist) */
  public cumple(regla: 'minLength' | 'uppercase' | 'lowercase' | 'number' | 'special'): boolean {
    const control = this.form.controls.password;
    return !!control.value && !control.errors?.[regla];
  }

  public noCoinciden(): boolean {
    const confirm = this.form.controls.passwordConfirm;
    return this.form.hasError('mismatch') && (confirm.touched || this.intentoEnviar());
  }

  public onRegister(): void {
    this.intentoEnviar.set(true);
    this.form.markAllAsTouched();

    if (this.form.invalid) return;

    this.isLoading.set(true);

    this.service.register$(this.form.getRawValue()).subscribe({
      next: async (res) => {
        this.isLoading.set(false);
        await this.alert.success('¡Registro exitoso!', 'Ya puedes iniciar sesión.');
        this.router.navigate(['/login']);
      },
      error: async (err) => {
        this.isLoading.set(false);
        const message = err?.error?.message ?? 'Inténtalo de nuevo en un momento.';
        await this.alert.error('No pudimos registrarte', message);
      }
    });
  }
}
