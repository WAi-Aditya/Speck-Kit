import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map, of, throwError } from 'rxjs';

const BASE = '/api/skill-matrix';

export interface CertificationDto {
  id: string;
  skillId: string;
  title: string;
  issuer: string;
  issueDate: string;
  expiryDate?: string;
  documentBlobUrl?: string;
}

export interface CategoryDto {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
  sortOrder: number;
}

export interface SubCategoryDto {
  id: string;
  skillCategoryId: string;
  name: string;
  description?: string;
  isActive: boolean;
  sortOrder: number;
}

export interface SkillDto {
  id: string;
  skillSubCategoryId: string;
  name: string;
  description?: string;
  isActive: boolean;
  sortOrder: number;
}

export interface PagedResult<T> {
  items: T[];
}

export interface ProfileSkillDto {
  employeeSkillId: string;
  skillId: string;
  skillName: string;
  categoryName: string;
  subCategoryName: string;
  selfAssessedLevel?: number;
  managerValidatedLevel?: number;
  finalRating?: number;
  validationStatus: number;
  validatedAt?: string;
}

export interface MyProfileResponse {
  employeeId: string;
  skills: ProfileSkillDto[];
}

export interface PendingApprovalItemDto {
  employeeSkillId: string;
  employeeId: string;
  employeeName: string;
  skillName: string;
  categoryName: string;
  subCategoryName: string;
  selfAssessedLevel?: number;
  submittedAt: string;
}

@Injectable({ providedIn: 'root' })
export class SkillService {
  constructor(private readonly http: HttpClient) {}

  getCategories(): Observable<PagedResult<CategoryDto>> {
    return this.http.get<PagedResult<CategoryDto>>(`${BASE}/categories`);
  }

  getCategory(id: string): Observable<CategoryDto | null> {
    return this.http.get<CategoryDto>(`${BASE}/categories/${id}`).pipe(
      map((c) => c ?? null)
    );
  }

  createCategory(body: { name: string; description?: string; sortOrder?: number }): Observable<CategoryDto> {
    return this.http.post<CategoryDto>(`${BASE}/categories`, body);
  }

  updateCategory(id: string, body: { name: string; description?: string; sortOrder?: number }): Observable<void> {
    return this.http.put<void>(`${BASE}/categories/${id}`, body);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${BASE}/categories/${id}`);
  }

  getSubCategories(categoryId?: string): Observable<PagedResult<SubCategoryDto>> {
    let params = new HttpParams();
    if (categoryId) params = params.set('categoryId', categoryId);
    return this.http.get<PagedResult<SubCategoryDto>>(`${BASE}/subcategories`, { params });
  }

  createSubCategory(body: {
    skillCategoryId: string;
    name: string;
    description?: string;
    sortOrder?: number;
  }): Observable<SubCategoryDto> {
    return this.http.post<SubCategoryDto>(`${BASE}/subcategories`, body);
  }

  updateSubCategory(
    id: string,
    body: { name: string; description?: string; sortOrder?: number }
  ): Observable<void> {
    return this.http.put<void>(`${BASE}/subcategories/${id}`, body);
  }

  getSkills(subcategoryId?: string): Observable<PagedResult<SkillDto>> {
    let params = new HttpParams();
    if (subcategoryId) params = params.set('subcategoryId', subcategoryId);
    return this.http.get<PagedResult<SkillDto>>(`${BASE}/skills`, { params });
  }

  createSkill(body: {
    skillSubCategoryId: string;
    name: string;
    description?: string;
    sortOrder?: number;
  }): Observable<SkillDto> {
    return this.http.post<SkillDto>(`${BASE}/skills`, body);
  }

  updateSkill(
    id: string,
    body: { name: string; description?: string; sortOrder?: number }
  ): Observable<void> {
    return this.http.put<void>(`${BASE}/skills/${id}`, body);
  }

  getMyProfile(): Observable<MyProfileResponse> {
    return this.http.get<MyProfileResponse>(`${BASE}/profile/me`);
  }

  addSkillToProfile(skillId: string, selfAssessedLevel: number): Observable<ProfileSkillDto> {
    return this.http.post<ProfileSkillDto>(`${BASE}/profile/me/skills`, {
      skillId,
      selfAssessedLevel,
    });
  }

  updateProfileSkill(employeeSkillId: string, selfAssessedLevel: number): Observable<void> {
    return this.http.patch<void>(`${BASE}/profile/me/skills/${employeeSkillId}`, {
      selfAssessedLevel,
    });
  }

  getPendingApprovals(): Observable<{ items: PendingApprovalItemDto[] }> {
    return this.http.get<{ items: PendingApprovalItemDto[] }>(`${BASE}/approvals/pending`);
  }

  approveOrReject(
    employeeSkillId: string,
    action: 'approve' | 'reject',
    managerValidatedLevel?: number,
    managerNotes?: string
  ): Observable<void> {
    return this.http.patch<void>(`${BASE}/approvals/${employeeSkillId}`, {
      action,
      managerValidatedLevel,
      managerNotes,
    });
  }

  getMyCertifications(): Observable<PagedResult<CertificationDto>> {
    return this.http.get<PagedResult<CertificationDto>>(`${BASE}/profile/me/certifications`);
  }

  uploadCertification(
    file: File,
    payload: { skillId: string; title: string; issuer: string; issueDate: string; expiryDate?: string }
  ): Observable<CertificationDto> {
    const form = new FormData();
    form.append('file', file);
    form.append('skillId', payload.skillId);
    form.append('title', payload.title);
    form.append('issuer', payload.issuer);
    form.append('issueDate', payload.issueDate);
    if (payload.expiryDate) form.append('expiryDate', payload.expiryDate);
    return this.http.post<CertificationDto>(`${BASE}/profile/me/certifications`, form);
  }

  getMyAssessments(): Observable<PagedResult<AssessmentDto>> {
    return this.http.get<PagedResult<AssessmentDto>>(`${BASE}/assessments/me`);
  }

  submitAssessment(assessmentId: string): Observable<void> {
    return this.http.patch<void>(`${BASE}/assessments/${assessmentId}/submit`, {});
  }
}

export interface AssessmentDto {
  id: string;
  employeeSkillId: string;
  assessmentType: number;
  status: number;
  dueDate?: string;
  skillName?: string;
}
