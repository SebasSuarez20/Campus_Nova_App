export interface StudentRow {
  idStudent: number;
  fullName: string;
  identification: number;
  email: string | null;
  subjectInscription: string;
  credits: number;
  isMine: boolean;
}
