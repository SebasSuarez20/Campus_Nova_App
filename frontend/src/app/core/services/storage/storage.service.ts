import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ResolveEnd } from '@angular/router';
import { retry } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  public set<T>(key: string, value: T): Promise<void> {
    if (this.isBrowser) {
      sessionStorage.setItem(key, JSON.stringify(value));
    }
    return Promise.resolve();
  }

  public get<T>(key: string): Promise<T | null> {

    const item = sessionStorage.getItem(key);
    return Promise.resolve(item ? JSON.parse(item) : null);
  }

   public remove(key: string): void {
      if (this.isBrowser) {
        sessionStorage.removeItem(key);
      }
    }

   public clear(): void {
      if (this.isBrowser) {
        sessionStorage.clear();
      }
    } 
}
