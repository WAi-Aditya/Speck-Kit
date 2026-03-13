import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

/**
 * Role names for Skill Matrix (constitution P4). Align with backend SkillMatrixPolicies.
 */
export const SkillMatrixRoles = {
  Employee: 'Employee',
  Manager: 'Manager',
  HRAdmin: 'HRAdmin',
  Leadership: 'Leadership',
} as const;

export type SkillMatrixRole = (typeof SkillMatrixRoles)[keyof typeof SkillMatrixRoles];

/**
 * Minimum role required for key areas. Used by guard and directive.
 */
export const SkillMatrixRoleRequirements = {
  profile: [SkillMatrixRoles.Employee],
  managerDashboard: [SkillMatrixRoles.Manager],
  adminTaxonomy: [SkillMatrixRoles.HRAdmin],
  reports: [SkillMatrixRoles.Manager, SkillMatrixRoles.Leadership],
} as const;

/**
 * Placeholder: get current user roles from your auth service (e.g. JWT claims or auth API).
 * Replace with actual injection of AuthService when identity is integrated.
 */
function getCurrentUserRoles(): string[] {
  // TODO: inject AuthService and return this.authService.getRoles();
  return [];
}

/**
 * Returns true if the user has at least one of the required roles.
 */
export function hasSkillMatrixRole(requiredRoles: readonly string[]): boolean {
  const userRoles = getCurrentUserRoles();
  return requiredRoles.some((r) => userRoles.includes(r));
}

/**
 * Route guard factory: requires one of the given roles to activate the route.
 * Usage in routes: canActivate: [skillMatrixRoleGuard(['Manager'])]
 */
export function skillMatrixRoleGuard(requiredRoles: readonly string[]): CanActivateFn {
  return () => {
    const router = inject(Router);
    if (hasSkillMatrixRole(requiredRoles)) return true;
    router.navigate(['/unauthorized']).catch(() => {});
    return false;
  };
}

/**
 * Predefined guards for Skill Matrix routes.
 */
export const SkillMatrixGuards = {
  employee: skillMatrixRoleGuard(SkillMatrixRoleRequirements.profile),
  manager: skillMatrixRoleGuard(SkillMatrixRoleRequirements.managerDashboard),
  hrAdmin: skillMatrixRoleGuard(SkillMatrixRoleRequirements.adminTaxonomy),
  reports: skillMatrixRoleGuard(SkillMatrixRoleRequirements.reports),
};
