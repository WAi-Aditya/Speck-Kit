import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { SkillMatrixPermissionDirective } from '../../core/auth/skill-matrix-permission.directive';
import { SkillMatrixRoles } from '../../core/auth/skill-matrix-permissions';

@Component({
  selector: 'app-skill-matrix-shell',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    RouterOutlet,
    SkillMatrixPermissionDirective,
  ],
  template: `
    <nav class="skill-matrix-nav">
      <a routerLink="profile" routerLinkActive="active">My Profile</a>
      <a *appSkillMatrixPermission="SkillMatrixRoles.Manager" routerLink="manager-dashboard" routerLinkActive="active">Approvals</a>
      <a *appSkillMatrixPermission="SkillMatrixRoles.HRAdmin" routerLink="admin-taxonomy" routerLinkActive="active">Taxonomy</a>
    </nav>
    <router-outlet></router-outlet>
  `,
  styles: [
    `
      .skill-matrix-nav {
        display: flex;
        gap: 1rem;
        padding: 0.5rem 1rem;
        border-bottom: 1px solid #e5e7eb;
        margin-bottom: 1rem;
      }
      .skill-matrix-nav a {
        text-decoration: none;
        color: #374151;
      }
      .skill-matrix-nav a.active {
        font-weight: 600;
        color: #111827;
      }
    `,
  ],
})
export class SkillMatrixShellComponent {
  SkillMatrixRoles = SkillMatrixRoles;
}
