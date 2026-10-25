export interface StudentClass {
  materiaId: number;
  name: string;
  code: string;
}

export interface Classmate {
  fullName: string;
  fullNameTeacher: string;
  idFkSubject: number;
}

export interface StudentClassesResult {
  subjects: StudentClass[];
  students: Classmate[];
}
