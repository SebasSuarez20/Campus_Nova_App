import { Component, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { StudentRow } from './students.model';
import { ModoPanel, StudentsService } from './students.service';
import { AlertService } from '../../core/services/alert/alert.service';
import { LogoutService } from '../../core/services/logout-session/logout.service';

/**
 * Directorio de estudiantes. Los datos y la lógica del formulario viven en
 * StudentsService; aquí solo queda el estado de la pantalla (panel y modal).
 */
@Component({
  selector: 'app-students',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './students.component.html',
  styleUrl: './students.component.css'
})
export class StudentsComponent {

  public readonly service = inject(StudentsService);
  private readonly alert = inject(AlertService);
  private readonly logout = inject(LogoutService);

  public readonly estudiantes = this.service.students;

  public readonly busqueda = signal('');
  public readonly filtrados = computed(() =>
    this.service.filterStudents(this.estudiantes(), this.busqueda())
  );
  public readonly form = this.service.createForm();

  public readonly panelAbierto = signal(false);
  public readonly modo = signal<ModoPanel>('ver');
  public readonly modalBajaAbierto = signal(false);
  public readonly seleccionado = signal<StudentRow | null>(null);
  public readonly intentoEnviar = signal(false);

  constructor() {
    this.service.getStudents$().subscribe();
  }

  /** El campo se marca como inválido después de tocarlo o de intentar guardar. */
  public invalido(campo: 'firstName' | 'lastName' | 'email' | 'identification' | 'password'): boolean {
    const control = this.form.controls[campo];
    return control.invalid && (control.touched || this.intentoEnviar());
  }

  public noCoinciden(): boolean {
    const confirm = this.form.controls.passwordConfirm;
    return this.form.hasError('mismatch') && (confirm.touched || this.intentoEnviar());
  }

  public buscar(event: Event): void {
    this.busqueda.set((event.target as HTMLInputElement).value);
  }

  public descargarPdf(): void {
    this.service.exportPdf(this.filtrados()).catch(async () => {
      await this.alert.error('No pudimos generar el PDF', 'Inténtalo de nuevo en un momento.');
    });
  }

  public abrirPanel(modo: ModoPanel, estudiante: StudentRow | null = null): void {
    this.modo.set(modo);
    this.seleccionado.set(estudiante);
    this.intentoEnviar.set(false);
    this.service.fillForm(this.form, modo, estudiante);
    this.panelAbierto.set(true);
  }

  public cerrarPanel(): void {
    this.panelAbierto.set(false);
  }

  public guardar(): void {
    this.intentoEnviar.set(true);
    this.form.markAllAsTouched();

    if (this.form.invalid || this.modo() !== 'editar') return;

    this.service.updateStudent$(this.form.getRawValue()).subscribe({
      next: async () => {
        this.cerrarPanel();
        await this.alert.success('Datos actualizados', 'Inicia sesión de nuevo para ver los cambios.');
        this.logout.logoutSessionComplete();
      },
      error: async (err) => {
        const message = err?.error?.message ?? 'Inténtalo de nuevo en un momento.';
        await this.alert.error('No pudimos guardar', message);
      }
    });
  }

  /** Elimina mi registro y cierra la sesión automáticamente. */
  public confirmarBaja(): void {
    this.service.deleteStudent$().subscribe({
      next: async () => {
        this.cerrarBaja();
        await this.alert.success('Registro eliminado', 'Cerramos tu sesión.');
        this.logout.logoutSessionComplete();
      },
      error: async (err) => {
        const message = err?.error?.message ?? 'Inténtalo de nuevo en un momento.';
        await this.alert.error('No pudimos eliminar tu registro', message);
      }
    });
  }

  public abrirBaja(estudiante: StudentRow): void {
    this.seleccionado.set(estudiante);
    this.modalBajaAbierto.set(true);
  }

  public cerrarBaja(): void {
    this.modalBajaAbierto.set(false);
  }
}
