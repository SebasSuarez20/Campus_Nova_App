import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
import { TopbarComponent } from './topbar/topbar.component';
import { IdleService } from '../services/idle/idle.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, TopbarComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit, OnDestroy {

  private readonly idleService = inject(IdleService);

  ngOnInit(): void {
    this.idleService.startWatching();
  }

  ngOnDestroy(): void {
    this.idleService.stopWatching();
  }
}
