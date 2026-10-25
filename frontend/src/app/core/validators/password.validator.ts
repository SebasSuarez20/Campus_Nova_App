import { AbstractControl, ValidationErrors } from '@angular/forms';

/**
 * Contraseña fuerte: mínimo 8 caracteres, con mayúscula, minúscula, número,
 * un símbolo y sin espacios. Si el campo está vacío no valida nada (para eso
 * está Validators.required). La misma regla se valida en el backend.
 */
export function strongPassword(control: AbstractControl): ValidationErrors | null {
  const value = control.value as string;
  if (!value) return null;

  const errors: ValidationErrors = {};

  if (value.length < 8)           errors['minLength'] = true;
  if (!/[A-Z]/.test(value))       errors['uppercase'] = true;
  if (!/[a-z]/.test(value))       errors['lowercase'] = true;
  if (!/[0-9]/.test(value))       errors['number'] = true;
  if (!/[^A-Za-z0-9\s]/.test(value)) errors['special'] = true;
  if (/\s/.test(value))           errors['spaces'] = true;

  return Object.keys(errors).length ? errors : null;
}
