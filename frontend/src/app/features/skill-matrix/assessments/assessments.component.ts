import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkillService, AssessmentDto } from '../data-access/skill.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-assessments',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule],
  template: `
    <div class="assessments">
      <h2>Assessments</h2>
      <p *ngIf="loading">Loading...</p>
      <p-table
        *ngIf="!loading"
        [value]="assessments"
        [tableStyle]="{ 'min-width': '50rem' }"
        styleClass="p-datatable-striped"
      >
        <ng-template pTemplate="header">
          <tr>
            <th>Skill</th>
            <th>Type</th>
            <th>Status</th>
            <th>Due</th>
            <th>Actions</th>
          </tr>
        </ng-template>
        <ng-template pTemplate="body" let-a>
          <tr>
            <td>{{ a.skillName ?? '-' }}</td>
            <td>{{ getTypeLabel(a.assessmentType) }}</td>
            <td>{{ getStatusLabel(a.status) }}</td>
            <td>{{ a.dueDate ?? '-' }}</td>
            <td>
              <button *ngIf="a.status === 0" pButton label="Submit" (click)="submit(a.id)"></button>
              <span *ngIf="a.status === 1">Pending evaluation</span>
            </td>
          </tr>
        </ng-template>
      </p-table>
    </div>
  `,
  styles: [`.assessments { padding: 1rem; }`],
})
export class AssessmentsComponent implements OnInit {
  assessments: AssessmentDto[] = [];
  loading = true;

  constructor(private readonly skillService: SkillService) {}

  ngOnInit(): void {
    this.loadAssessments();
  }

  loadAssessments(): void {
    this.loading = true;
    this.skillService.getMyAssessments().subscribe({
      next: (r) => {
        this.assessments = r?.items ?? [];
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  getTypeLabel(type: number): string {
    const labels: Record<number, string> = { 0: 'Self', 1: 'Manager', 2: 'Peer', 3: 'System' };
    return labels[type] ?? 'Unknown';
  }

  getStatusLabel(status: number): string {
    const labels: Record<number, string> = { 0: 'Assigned', 1: 'Submitted', 2: 'Evaluated' };
    return labels[status] ?? 'Unknown';
  }

  submit(assessmentId: string): void {
    this.skillService.submitAssessment(assessmentId).subscribe({
      next: () => this.loadAssessments(),
      error: () => {},
    });
  }
}
