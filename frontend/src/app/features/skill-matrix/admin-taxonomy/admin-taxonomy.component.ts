import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SkillService, CategoryDto, SubCategoryDto, SkillDto } from '../data-access/skill.service';

@Component({
  selector: 'app-admin-taxonomy',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, InputTextModule],
  templateUrl: './admin-taxonomy.component.html',
  styleUrl: './admin-taxonomy.component.css',
})
export class AdminTaxonomyComponent implements OnInit {
  categories: CategoryDto[] = [];
  subCategories: SubCategoryDto[] = [];
  skills: SkillDto[] = [];
  loading = true;

  constructor(private readonly skillService: SkillService) {}

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.loading = true;
    this.skillService.getCategories().subscribe({
      next: (r) => {
        this.categories = r.items ?? [];
        this.skillService.getSubCategories().subscribe({
          next: (sr) => {
            this.subCategories = sr.items ?? [];
            this.skillService.getSkills().subscribe({
              next: (kr) => {
                this.skills = kr.items ?? [];
                this.loading = false;
              },
              error: () => (this.loading = false),
            });
          },
          error: () => (this.loading = false),
        });
      },
      error: () => (this.loading = false),
    });
  }

  getSubsForCategory(categoryId: string): SubCategoryDto[] {
    return this.subCategories.filter((s) => s.skillCategoryId === categoryId);
  }

  getSkillsForSub(subId: string): SkillDto[] {
    return this.skills.filter((s) => s.skillSubCategoryId === subId);
  }

  getCategoryName(categoryId: string): string {
    const cat = this.categories.find((c) => c.id === categoryId);
    return cat?.name ?? categoryId;
  }

  getSubCategoryName(subCategoryId: string): string {
    const sub = this.subCategories.find((s) => s.id === subCategoryId);
    return sub?.name ?? subCategoryId;
  }
}
