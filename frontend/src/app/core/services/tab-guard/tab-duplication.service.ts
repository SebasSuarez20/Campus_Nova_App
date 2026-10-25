import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TabDuplicationService {
  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  private isDuplicateSubject = new BehaviorSubject<boolean>(false);
  public isDuplicate$ = this.isDuplicateSubject.asObservable();

  private channel?: BroadcastChannel;
  private myInstanceId = Math.random().toString(36).substring(2, 11) + '_' + Date.now();
  private myTabId = '';

  constructor() {
    this.initTabCheck();
  }

  private initTabCheck() {
    const storedTabId = sessionStorage.getItem('tab_id');

    if (!storedTabId) {
      // It's a completely new tab session. Create a new tabId.
      this.myTabId = 'tab_' + Math.random().toString(36).substring(2, 11) + '_' + Date.now();
      sessionStorage.setItem('tab_id', this.myTabId);
      this.setupChannel();
    } else {
      this.myTabId = storedTabId;
      this.setupChannel();
      this.pingTabs();
    }
  }

  private setupChannel() {
    try {
      this.channel = new BroadcastChannel('nova_tab_channel');

      this.channel.onmessage = (event) => {
        const data = event.data;
        if (!data) return;

        if (data.type === 'PING_TAB') {
          if (data.tabId === this.myTabId) {

            this.channel?.postMessage({
              type: 'PONG_TAB',
              tabId: this.myTabId
            });
          }
        }

        if (data.type === 'PONG_TAB') {
          if (data.tabId === this.myTabId) {
            this.isDuplicateSubject.next(true);
          }
        }
      };
    } catch (e) {
      console.warn('BroadcastChannel is not supported or failed to initialize:', e);
    }
  }

  private pingTabs() {
    this.channel?.postMessage({
      type: 'PING_TAB',
      tabId: this.myTabId
    });
  }
}
