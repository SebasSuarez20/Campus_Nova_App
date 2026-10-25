import { inject, Injectable } from '@angular/core';
import { AbstractControl, FormBuilder, ValidationErrors, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/http-client/api.service';
import { strongPassword } from '../../core/validators/password.validator';

/** Error de grupo: la confirmación debe ser igual a la contraseña. */
function passwordsMatch(group: AbstractControl): ValidationErrors | null {
  const password = group.get('password')?.value;
  const confirm = group.get('passwordConfirm')?.value;
  return password === confirm ? null : { mismatch: true };
}

@Injectable({ providedIn: 'root' })
export class RegisterService {

  private readonly api = inject(ApiService);
  private readonly fb = inject(FormBuilder);

  public createForm() {
    return this.fb.nonNullable.group({
      institutionCode: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      firstName:       ['', [Validators.required, Validators.maxLength(100)]],
      lastName:        ['', [Validators.required, Validators.maxLength(100)]],
      email:           ['', [Validators.required, Validators.email, Validators.maxLength(120)]],
      identification:  ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      password:        ['', [Validators.required, strongPassword]],
      passwordConfirm: ['', [Validators.required]]
    }, { validators: passwordsMatch });
  }

  /** Crea el estudiante y su usuario. Es público: no necesita sesión. */
  public register$(raw: {
    institutionCode: string; firstName: string; lastName: string;
    email: string; identification: string; password: string;
  }) {
    const body = {
      institutionCode: Number(raw.institutionCode),
      student: {
        firstName: raw.firstName.trim(),
        lastName: raw.lastName.trim(),
        email: raw.email.trim(),
        identification: Number(raw.identification),
        password: raw.password
      }
    };

    return this.api.add<string, typeof body>('Student/create-student', body);
  }
}
