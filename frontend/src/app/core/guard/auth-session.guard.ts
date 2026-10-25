import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthSessionService } from '../services/auth-session/auth-session.service';
import { RolGuardService } from '../services/rolGuard/rol-guard.service';
import { StorageService } from '../services/storage/storage.service';
import { session } from '../entities/session.entities';

/**
 * Protege las pantallas internas: primero exige sesión iniciada y después
 * que el rol del usuario tenga permiso para la acción de la ruta (data.action).
 */
export const authSessionGuard: CanActivateFn = async (route) => {
  const auth = inject(AuthSessionService);
  const router = inject(Router);
  const rolGuard = inject(RolGuardService);
  const storage = inject(StorageService);

  if (!auth.sessionAuthentication()) {
    return router.createUrlTree(['/login']);
  }

  // Sesión dañada o vieja (sin datos o sin rol): se limpia y se vuelve a iniciar sesión.
  const data = await storage.get<session>('dta_session');
  if (!data?.rol) {
    storage.clear();
    return router.createUrlTree(['/login']);
  }

  const action = route.data['action'] as string;
  const hasRol = await rolGuard.checkRole(action);

  if (!hasRol) {
    return router.createUrlTree(['/access-denied']);
  }

  return true;
};

/** Evita volver al login si ya hay una sesión abierta. */
export const logguedSessionGuard: CanActivateFn = () => {
  const auth = inject(AuthSessionService);
  const router = inject(Router);

  if (auth.sessionAuthentication()) {
    router.navigate(['pages/subjects']);
    return false;
  }

  return true;
};
