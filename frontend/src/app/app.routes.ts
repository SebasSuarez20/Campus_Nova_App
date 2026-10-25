import { Routes } from '@angular/router';
import { logguedSessionGuard } from './core/guard/auth-session.guard';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [logguedSessionGuard],
    loadChildren: () => import('./features/login/login.routes').then(m => m.LOGIN_ROUTES)
  },
  {
    path: 'registro',
    loadComponent: () => import('./features/register/register.component').then(m => m.RegisterComponent)
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'pages',
    loadComponent: () => import('./core/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: 'subjects',
        loadComponent: () => import('./features/subjects/subjects.component').then(m => m.SubjectsComponent),
        data: { action: 'Subjects' }
      },
      {
        path: 'my-classes',
        loadComponent: () => import('./features/my-classes/my-classes.component').then(m => m.MyClassesComponent),
        data: { action: 'MyClasses' }
      },
      {
        path: 'students',
        loadComponent: () => import('./features/students/students.component').then(m => m.StudentsComponent),
        data: { action: 'Students' }
      },
      { path: '', redirectTo: 'subjects', pathMatch: 'full' }
    ]
  },
  {
    path: 'access-denied',
    loadComponent: () => import('./features/access-denied/access-denied.component').then(m => m.AccessDeniedComponent)
  },
  { path: '**', redirectTo: 'access-denied' }
];
