import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { RolGuardService } from '../services/rolGuard/rol-guard.service';

/**
 * Muestra el elemento solo si el rol de la sesión tiene permiso para la acción.
 * Uso: *appHasRole="'Subjects'"
 */
@Directive({
  selector: '[appHasRole]',
  standalone: true
})
export class HasRoleDirective {
  private templateRef = inject(TemplateRef);
  private viewContainer = inject(ViewContainerRef);
  private rolGuard = inject(RolGuardService);

  @Input() set appHasRole(viewAction: string) {
    this.checkRole(viewAction);
  }

  private async checkRole(action: string) {
    const hasAccess = await this.rolGuard.checkRole(action);

    this.viewContainer.clear();
    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }
}
