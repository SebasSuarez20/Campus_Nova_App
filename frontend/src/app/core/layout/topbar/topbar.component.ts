import { Component, OnInit, inject } from '@angular/core';
import { StorageService } from '../../services/storage/storage.service';
import { session } from '../../entities/session.entities';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.css'
})
export class TopbarComponent implements OnInit {

  private readonly storage = inject(StorageService);

  // Valores por defecto para ver el diseño sin sesión; se pisan con la sesión real.
  public fullName = 'Camila Torres';
  public rol = 'Estudiante';
  public initials = 'CT';

  async ngOnInit(): Promise<void> {
    const data = await this.storage.get<session>('dta_session');
    if (!data) return;

    this.fullName = `${data.firstName} ${data.lastName}`.trim();
    this.rol = 'Estudiante';
    this.initials = `${data.firstName?.[0] ?? ''}${data.lastName?.[0] ?? ''}`.toUpperCase();
  }
}
