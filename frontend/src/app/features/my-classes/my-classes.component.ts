import { Component, inject, signal } from '@angular/core';
import { MyClassesService } from './my-classes.service';

/**
 * Mis clases: las materias inscritas y, al elegir una, los compañeros con quienes
 * comparte clase. Regla de negocio: solo se muestra el NOMBRE de cada compañero.
 */
@Component({
  selector: 'app-my-classes',
  standalone: true,
  imports: [],
  templateUrl: './my-classes.component.html',
  styleUrl: './my-classes.component.css'
})
export class MyClassesComponent {

  private readonly subjectService = inject(MyClassesService);

  public informationClasess = this.subjectService.classesFull;
  public informationClassmates = this.subjectService.classmatesFull;

  public isActiveFiltro = signal<boolean>(false);
  public selectedId = signal<number | null>(null);

  constructor(){
    this.subjectService.getMyClasses$().subscribe();
  }

  /** Cuántos compañeros hay en una materia. */
  public companeros(materiaId: number): number {
    return this.subjectService.classmatesFullCopy()?.filter(s => s.idFkSubject === materiaId).length ?? 0;
  }

  public iniciales(nombre: string): string {
    const partes = nombre.trim().split(/\s+/);
    return `${partes[0]?.[0] ?? ''}${partes[1]?.[0] ?? ''}`.toUpperCase();
  }

  public elegir(materiaId: number): void {
    const students = this.subjectService.classmatesFullCopy()?.filter(s => s.idFkSubject === materiaId);
    this.informationClassmates.set(students ?? []);
    this.selectedId.set(materiaId);
    this.isActiveFiltro.set(true);
  }
}
