import { inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { ApiService } from '../../core/services/http-client/api.service';
import { Classmate, StudentClass, StudentClassesResult } from './my-classes.model';

@Injectable({ providedIn: 'root' })
export class MyClassesService {

  private readonly api = inject(ApiService);

  public classesFull = signal<StudentClass[] | null>(null);
  public classmatesFull = signal<Classmate[] | null>(null);
  public classmatesFullCopy = signal<Classmate[] | null>(null);

  public getMyClasses$() {
    return this.api.get<StudentClassesResult>('Subject/get-student-classes').pipe(
      tap(res => {
        this.classesFull.set(res.data?.subjects ?? null);
        this.classmatesFull.set(res.data?.students ?? null);
        this.classmatesFullCopy.set([...this.classmatesFull() ?? []]);
      })
    );
  }
}
