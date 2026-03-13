# Skill Matrix API Contract

**Base path**: `/api/skill-matrix` (or as configured). **Auth**: Bearer token; roles: Employee, Manager, HRAdmin, Leadership.  
**Error format**: RFC 7807 Problem Details (application/problem+json). All 4xx/5xx return a body with `type`, `title`, `status`, `detail`, optional `errors` (validation).

---

## Taxonomy (HR Admin only)

| Method | Path | Description | Request | Response | Status |
|--------|------|-------------|---------|----------|--------|
| GET | /categories | List categories | — | `{ "items": [{ "id", "name", "description", "isActive", "sortOrder" }] }` | 200 |
| POST | /categories | Create category | `{ "name", "description?", "sortOrder?" }` | `{ "id", ... }` | 201, 400 (validation), 403 |
| GET | /categories/{id} | Get category | — | Category object | 200, 404 |
| PUT | /categories/{id} | Update category | Same as POST | — | 204, 400, 403, 404 |
| DELETE | /categories/{id} | Deactivate (soft) | — | — | 204, 403, 404, 409 if in use |
| GET | /subcategories | List by category | ?categoryId= | `{ "items": [{ "id", "skillCategoryId", "name", ... }] }` | 200 |
| POST | /subcategories | Create subcategory | `{ "skillCategoryId", "name", "description?", "sortOrder?" }` | 201, 400, 403, 404 (invalid category) |
| GET | /skills | List by subcategory | ?subcategoryId= | `{ "items": [{ "id", "skillSubCategoryId", "name", ... }] }` | 200 |
| POST | /skills | Create skill | `{ "skillSubCategoryId", "name", "description?", "sortOrder?" }` | 201, 400, 403, 404 |

- **Validation**: name required, max length per data-model; category/subcategory must exist and be active. Duplicate name in same parent → 400 with `errors`.

---

## Employee Skill Profile (Employee: own only; Manager: team)

| Method | Path | Description | Request | Response | Status |
|--------|------|-------------|---------|----------|--------|
| GET | /profile/me | Current user's skill profile | — | `{ "employeeId", "skills": [{ "skillId", "skillName", "categoryName", "subCategoryName", "selfAssessedLevel", "managerValidatedLevel", "finalRating", "validationStatus", "validatedAt?" }] }` | 200 |
| POST | /profile/me/skills | Add skill to profile | `{ "skillId", "selfAssessedLevel": 1-4 }` | 201 + body, 400, 403, 404 (skill not found/inactive) |
| PATCH | /profile/me/skills/{employeeSkillId} | Update proficiency | `{ "selfAssessedLevel": 1-4 }` | 204, 400, 403, 404 |
| GET | /profile/{employeeId} | Get profile (Manager: report only) | — | Same shape as /profile/me | 200, 403, 404 |

- **Validation**: skillId must exist and be active; selfAssessedLevel 1–4. Ownership: employee can only change own profile; manager read-only for reports.

---

## Manager Approval (Manager only)

| Method | Path | Description | Request | Response | Status |
|--------|------|-------------|---------|----------|--------|
| GET | /approvals/pending | Pending items for my reports | — | `{ "items": [{ "employeeSkillId", "employeeId", "employeeName", "skillName", "selfAssessedLevel", "submittedAt", ... }] }` | 200 |
| PATCH | /approvals/{employeeSkillId} | Approve or reject | `{ "action": "approve" | "reject", "managerValidatedLevel?": 1-4, "managerNotes?" }` | 204, 400, 403, 404 |

- **Validation**: action required; if approve, managerValidatedLevel 1–4 optional (defaults to self level). Manager may only approve/reject for direct or delegated reports.

---

## Certifications (Employee: own)

| Method | Path | Description | Request | Response | Status |
|--------|------|-------------|---------|----------|--------|
| POST | /profile/me/certifications | Upload certification | multipart/form-data: file, skillId, title, issuer, issueDate, expiryDate? | 201 + `{ "id", "documentBlobUrl", ... }`, 400, 403, 404 |
| GET | /profile/me/certifications | List my certifications | — | `{ "items": [{ "id", "skillId", "title", "issuer", "issueDate", "expiryDate?", "documentBlobUrl?" }] }` | 200 |

- **Validation**: issueDate required; if expiryDate provided, expiryDate >= issueDate. File type/size per config.

---

## Reporting (Manager / Leadership)

| Method | Path | Description | Request | Response | Status |
|--------|------|-------------|---------|----------|--------|
| GET | /reports/gap-analysis | Skill gap | ?departmentId=&teamId=&categoryId= | `{ "gaps": [{ "skillId", "skillName", "requiredLevel", "currentAvg", "employeeCount" }] }` | 200, 403 |
| GET | /reports/team-distribution | Team distribution | ?teamId=&categoryId= | `{ "byLevel": [{ "level", "count" }], "bySkill": [...] }` | 200, 403 |
| GET | /reports/heatmap | Org heatmap | ?departmentId=&categoryId= | `{ "rows": [...], "columns": [...] }` | 200, 403 |
| GET | /reports/project-matching | Match employees to project | ?projectId=&skillIds= | `{ "candidates": [{ "employeeId", "matchPercentage", "skillMatches": [...] }] }` | 200, 403, 404 |

---

## Error response shape (RFC 7807)

```json
{
  "type": "https://api.itp.com/errors/validation",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "errors": { "SelfAssessedLevel": ["Must be between 1 and 4."] }
}
```

- 401 Unauthorized: missing or invalid token.  
- 403 Forbidden: valid user but not allowed for this resource.  
- 404 Not Found: resource does not exist or user has no access.  
- 409 Conflict: e.g. duplicate skill on profile, delete category in use.
