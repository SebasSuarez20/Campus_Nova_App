export interface Subject {
  code: string;
  name: string;
  description: string | null;
  fullName: string;
  credits: number;
  totalStudents: number;
  professorId: number;
  subjectId: number;
  result: string;
}

export interface Professor {
  professorId: number;
  fullName: string;
}

export interface SubjectsResult {
  subjects: Subject[];
  professors: Professor[];
}
