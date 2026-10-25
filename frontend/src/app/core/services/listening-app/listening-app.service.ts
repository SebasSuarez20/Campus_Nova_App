import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ListeningAppService {

  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));
  private listening = new Subject<string>();

  constructor() { }

  public listeningObservable():Observable<string>{
      return this.listening.asObservable();
  }

  public setListeningObservable(message:string) :void{
      this.listening.next(message);
  }


}

