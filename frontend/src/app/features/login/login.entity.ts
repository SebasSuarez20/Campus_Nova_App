/**
 * Lo que se manda al backend. Ambos identificadores son `long` en .NET
 * (LoginQuery.cs), así que viajan como número, no como texto.
 */
export interface LoginRequest {
  institutionCode: number;
  username: number;
  password: string;
}

/** Lo que produce el formulario reactivo: todo texto. */
export interface LoginFormValue {
  institutionCode: string;
  username: string;
  password: string;
}
