import { HttpInterceptorFn, HttpStatusCode } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { ApiService } from '../services/http-client/api.service';
import { LogoutService } from '../services/logout-session/logout.service';
import { StorageService } from '../services/storage/storage.service';

const REFRESH_URL = 'Refresh/refresh-token';
const RETRY_HEADER = 'X-Retry-After-Refresh';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(ApiService);
  const router      = inject(Router);
  const logoutUser = inject(LogoutService);
  const storage     = inject(StorageService);

  const reqConCookie = req.clone({ withCredentials: true });

  if (req.url.includes(REFRESH_URL) || req.headers.has(RETRY_HEADER)) {
    return next(reqConCookie).pipe(
      catchError(error => {
        if (error.status === HttpStatusCode.Unauthorized || error.status === HttpStatusCode.Forbidden) {
          storage.clear();
          router.navigate(['/login']);
        }
        return throwError(() => error);
      })
    );
  }

  return next(reqConCookie).pipe(
    catchError(error => {

      if (error.status === HttpStatusCode.Forbidden) {
        logoutUser.logoutService();
        storage.clear();
        router.navigate(['/login']);
        return throwError(() => error);
      }

      if (error.status !== HttpStatusCode.Unauthorized) {
        return throwError(() => error);
      }

      return authService.add(REFRESH_URL, {}).pipe(
        switchMap(() => {

          console.log("Refresco exitosamente !!");

          // Crea un request completamente nuevo con las cookies frescas
          const retryReq = req.clone({
            withCredentials: true,
            setHeaders: { [RETRY_HEADER]: 'true' }
          });

          return next(retryReq);
        }),
        catchError((retryErr) => {
          storage.clear();
          router.navigate(['/login']);
          return throwError(() => retryErr);
        })
      );
    })
  );
};