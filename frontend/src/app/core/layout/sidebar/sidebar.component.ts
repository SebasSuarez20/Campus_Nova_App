import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { SignalrService } from '../../services/signalR/signalr.service';
import { LogoutService } from '../../services/logout-session/logout.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit, OnDestroy {

  private readonly signalR = inject(SignalrService);
  private readonly logoutUser = inject(LogoutService);
  private readonly router = inject(Router);
  private readonly destroy$ = new Subject<void>();

  ngOnInit(): void {
    this.signalR.startConnection();
    this.listeningNotifications();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public logout(): void {
    this.logoutUser.logoutSessionComplete();
  }

  /** El backend empuja ForceLogout cuando detecta una segunda sesión del mismo usuario. */
  private listeningNotifications(): void {
    this.signalR.listeningObservable$
      .asObservable()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.logoutUser.logoutSessionComplete()
      });
  }
}
