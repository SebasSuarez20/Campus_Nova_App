import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from './auth.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  public loginForm: FormGroup;
  public showPassword = false;
  public isLoading = false;

  constructor() {
    this.loginForm = this.fb.group({
      institutionCode: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      username:    ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      password:    ['', Validators.required],
    });
  }

  public get institutionCode() { return this.loginForm.get('institutionCode')!; }
  public get username()    { return this.loginForm.get('username')!; }
  public get password()    { return this.loginForm.get('password')!; }

  public togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  public onLogin(): void {
  
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.authService.login(this.loginForm.value, () => this.isLoading = false);
  }
}
