import { inject, Injectable, signal } from '@angular/core';
import { Professor, Subject, SubjectsResult } from './subject.model';
import { AlertService } from '../../core/services/alert/alert.service';
import { ApiService } from '../../core/services/http-client/api.service';
import { EMPTY, of, switchMap, tap } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class SubjectService {

  private readonly alert = inject(AlertService);
  public subjectFullCopy:Subject[] | null = null;
  public subjectFull = signal<Subject[] | null>(null);
  public professors = signal<Professor[]>([]);
  public idFilter:number | null = null;
  private readonly api = inject(ApiService);


  public getSubjects$() {
  return this.api.get<SubjectsResult>('Professor/get-subjects').pipe(
    tap(res => {
      this.subjectFull.set(res.data?.subjects ?? null);
      this.subjectFullCopy = res.data?.subjects ?? null;
      this.professors.set(res.data?.professors ?? []);
    })
  );
}

public async saveSubjectComplete(id: string | null, url: string | null = null) {
  const confirm = url == null
    ? { isConfirmed: true }
    : await this.alert.confirm('¿Desea eliminar la materia?', 'Esta acción no se puede deshacer.');

  if (!confirm.isConfirmed) return EMPTY;

  const request$ = url == null
    ? this.api.add('Subject/create-subject', { SubjectCode: id })
    : this.api.updateOurParams(url, { SubjectCode: id });

  return request$.pipe(switchMap(() => this.getSubjects$()));
}


}
