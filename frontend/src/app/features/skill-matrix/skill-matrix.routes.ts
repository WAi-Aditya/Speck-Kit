import { Routes } from '@angular/router';
import { SkillMatrixGuards } from '../../core/auth/skill-matrix-permissions';
import { SkillMatrixShellComponent } from './skill-matrix-shell.component';

export const SKILL_MATRIX_ROUTES: Routes = [
  {
    path: '',
    component: SkillMatrixShellComponent,
    children: [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'profile',
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./skill-profile/skill-profile.component').then((m) => m.SkillProfileComponent),
    canActivate: [SkillMatrixGuards.employee],
    title: 'My Skill Profile',
  },
  {
    path: 'manager-dashboard',
    loadComponent: () =>
      import('./manager-dashboard/manager-dashboard.component').then((m) => m.ManagerDashboardComponent),
    canActivate: [SkillMatrixGuards.manager],
    title: 'Approvals',
  },
  {
    path: 'admin-taxonomy',
    loadComponent: () =>
      import('./admin-taxonomy/admin-taxonomy.component').then((m) => m.AdminTaxonomyComponent),
    canActivate: [SkillMatrixGuards.hrAdmin],
    title: 'Taxonomy',
  },
    ],
  },
];
