import { inject, Injectable } from '@angular/core';
import { StorageService } from '../storage/storage.service';
import { session } from '../../entities/session.entities';
import { RoleStrategy } from '../../entities/role-strategy.interface';

@Injectable({
  providedIn: 'root'
})
export class RolGuardService {

  private sessionActive = inject(StorageService);
  private strategy: RoleStrategy;

  constructor() {
    this.strategy = new RoleStrategyLocal();
  }

  setStrategy(strategy: RoleStrategy) {
    this.strategy = strategy;
  }

  /** ¿El rol de la sesión tiene permiso para esta acción? Sin sesión devuelve false. */
  public async checkRole(action: string): Promise<boolean> {
    const data = await this.sessionActive.get<session>("dta_session");
    if (!data?.rol) return false;
    return this.strategy.hasAccess(data.rol, action);
  }
}

export class RoleStrategyLocal implements RoleStrategy {
  private readonly allowedRoles: Record<string, string[]> = {
    "Admin": ["Subjects", "MyClasses", "Students"],
    "Student": ["Subjects", "MyClasses", "Students"]
  };

  hasAccess(role: string, action: string): boolean {
    return this.allowedRoles[role]?.includes(action) ?? false;
  }
}
