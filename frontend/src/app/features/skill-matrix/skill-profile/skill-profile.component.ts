import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { SkillService, MyProfileResponse, ProfileSkillDto } from '../data-access/skill.service';

@Component({
  selector: 'app-skill-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, DropdownModule],
  templateUrl: './skill-profile.component.html',
  styleUrl: './skill-profile.component.css',
})
export class SkillProfileComponent implements OnInit {
  profile: MyProfileResponse | null = null;
  loading = true;
  levelOptions = [
    { label: 'Beginner', value: 1 },
    { label: 'Intermediate', value: 2 },
    { label: 'Advanced', value: 3 },
    { label: 'Expert', value: 4 },
  ];
  editingId: string | null = null;
  saveError: string | null = null;

  constructor(private readonly skillService: SkillService) {}

  ngOnInit(): void {
    this.skillService.getMyProfile().subscribe({
      next: (p) => {
        this.profile = p;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return '';
    }
  }

  startEdit(row: ProfileSkillDto): void {
    this.editingId = row.employeeSkillId;
    this.saveError = null;
  }

  cancelEdit(): void {
    this.editingId = null;
    this.saveError = null;
  }

  saveLevel(row: ProfileSkillDto, level: number): void {
    if (!row || level < 1 || level > 4) return;
    this.skillService.updateProfileSkill(row.employeeSkillId, level).subscribe({
      next: () => {
        if (this.profile)
          this.profile = {
            ...this.profile,
            skills: this.profile.skills.map((s) =>
              s.employeeSkillId === row.employeeSkillId ? { ...s, selfAssessedLevel: level } : s
            ),
          };
        this.editingId = null;
        this.saveError = null;
      },
      error: (err) => {
        this.saveError = err?.error?.detail || 'Save failed';
      },
    });
  }
}
