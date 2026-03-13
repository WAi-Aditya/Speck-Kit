import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  inject,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { hasSkillMatrixRole, SkillMatrixRole } from './skill-matrix-permissions';

/**
 * Structural directive to show/hide content based on Skill Matrix role.
 * Usage: *appSkillMatrixPermission="['Manager']" or *appSkillMatrixPermission="'HRAdmin'"
 */
@Directive({
  selector: '[appSkillMatrixPermission]',
  standalone: true,
})
export class SkillMatrixPermissionDirective implements OnChanges {
  private readonly templateRef = inject(TemplateRef<unknown>);
  private readonly viewContainer = inject(ViewContainerRef);

  @Input() set appSkillMatrixPermission(roles: SkillMatrixRole | SkillMatrixRole[] | null | undefined) {
    this.allowedRoles = Array.isArray(roles) ? roles : roles ? [roles] : [];
    this.updateView();
  }

  private allowedRoles: string[] = [];

  ngOnChanges(_changes: SimpleChanges): void {
    this.updateView();
  }

  private updateView(): void {
    this.viewContainer.clear();
    if (this.allowedRoles.length > 0 && hasSkillMatrixRole(this.allowedRoles)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }
}
