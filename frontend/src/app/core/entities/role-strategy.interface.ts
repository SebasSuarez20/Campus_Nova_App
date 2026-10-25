export interface RoleStrategy {
  hasAccess(role: string, action: string): boolean;
}