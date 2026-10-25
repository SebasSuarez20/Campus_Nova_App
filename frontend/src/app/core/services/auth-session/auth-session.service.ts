import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthSessionService {
  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  constructor() { }

  public sessionAuthentication(): boolean {
     if (!this.isBrowser) {
       return false;
     }
     return Boolean(sessionStorage.getItem("dta_active_session"));
  }

}
