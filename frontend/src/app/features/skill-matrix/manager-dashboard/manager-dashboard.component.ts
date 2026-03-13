import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { SkillService, PendingApprovalItemDto } from '../data-access/skill.service';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, DialogModule, InputTextareaModule, DropdownModule],
  templateUrl: './manager-dashboard.component.html',
  styleUrl: './manager-dashboard.component.css',
})
export class ManagerDashboardComponent implements OnInit {
  items: PendingApprovalItemDto[] = [];
  loading = true;
  selectedRow: PendingApprovalItemDto | null = null;
  dialogVisible = false;
  action: 'approve' | 'reject' = 'approve';
  managerLevel = 2;
  managerNotes = '';
  levelOptions = [
    { label: 'Beginner', value: 1 },
    { label: 'Intermediate', value: 2 },
    { label: 'Advanced', value: 3 },
    { label: 'Expert', value: 4 },
  ];
  submitError: string | null = null;

  constructor(private readonly skillService: SkillService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.skillService.getPendingApprovals().subscribe({
      next: (r) => {
        this.items = r.items ?? [];
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  openDialog(row: PendingApprovalItemDto, action: 'approve' | 'reject'): void {
    this.selectedRow = row;
    this.action = action;
    this.managerLevel = row.selfAssessedLevel ?? 2;
    this.managerNotes = '';
    this.submitError = null;
    this.dialogVisible = true;
  }

  submit(): void {
    if (!this.selectedRow) return;
    const level = this.action === 'approve' ? this.managerLevel : undefined;
    this.skillService
      .approveOrReject(this.selectedRow.employeeSkillId, this.action, level, this.managerNotes)
      .subscribe({
        next: () => {
          this.dialogVisible = false;
          this.selectedRow = null;
          this.load();
        },
        error: (err) => {
          this.submitError = err?.error?.detail || 'Action failed';
        },
      });
  }
}
