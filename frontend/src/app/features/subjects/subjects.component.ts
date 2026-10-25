import { Component, computed, inject, signal } from '@angular/core';
import { SubjectService } from './subject.service';
import { Subject } from './subject.model';
import { AlertService } from '../../core/services/alert/alert.service';


const TONOS = ['violet', 'coral', 'mint', 'sky', 'amber'];

@Component({
  selector: 'app-subjects',
  standalone: true,
  imports: [],
  templateUrl: './subjects.component.html',
  styleUrl: './subjects.component.css'
})
export class SubjectsComponent {

  private readonly subjectService = inject(SubjectService);
  public readonly filterTeacher = this.subjectService.subjectFull;
  public readonly teachers = computed(() =>
    this.subjectService.professors().map((p, i) => ({
      id: p.professorId,
      nombre: p.fullName,
      tono: TONOS[i % TONOS.length]
    }))
  );
    private readonly alert = inject(AlertService);
  
  public readonly visibles = computed(() => {
    const filtro = this.filterTeacher();
    return filtro;
  });

  constructor(){
    this.subjectService.getSubjects$().subscribe();
  }

  public filtrar(id: number | null): void {
    this.filterInformation(id);   
    this.subjectService.idFilter = id;
  }

  private filterInformation(id: number | null): void {
    if (id === null) {
      this.filterTeacher.set(this.subjectService.subjectFullCopy);
      return;
    }
    const filterInfo = this.subjectService.subjectFullCopy?.filter((m) => m.professorId === id) ?? null;
    this.filterTeacher.set(filterInfo);
  }

  public async saveSubject(id:number | null,status:string) {

  (await this.subjectService.
    saveSubjectComplete(String(id), status === 'Completado' ? 'Subject/delete-subject' : null))?.subscribe({
      next:() =>{
        this.filterInformation(this.subjectService.idFilter);
      },
      error:(err)=>{
       this.alert.error('No pudimos inscribirte', err.error.message);
      }
    });
    this.filterInformation(this.subjectService.idFilter);
  }
}
