# MVP Regression Test Suite: Skill Matrix User Story 1

**Purpose**: Validate taxonomy, employee profile, and manager approval flows before story closure.  
**Feature**: specs/001-skill-matrix (User Story 1).

## Scope

- Taxonomy CRUD (categories, subcategories, skills) — HR Admin only
- Employee skill profile (get my profile, add skill, update proficiency) — Employee
- Manager approval (pending list, approve/reject) — Manager only
- Role restrictions enforced (403 for unauthorized roles)

## Test Checklist

### Taxonomy (Contract + API)

- [ ] GET /api/skill-matrix/categories returns 200 and `items` array
- [ ] GET /api/skill-matrix/categories/{id} returns 200 when exists, 404 when not
- [ ] POST /api/skill-matrix/categories returns 201 with created category (name required, max 200)
- [ ] PUT /api/skill-matrix/categories/{id} returns 204; 400 for duplicate name
- [ ] DELETE /api/skill-matrix/categories/{id} returns 204 or 409 if in use
- [ ] GET /api/skill-matrix/subcategories?categoryId= returns 200 and items
- [ ] POST /api/skill-matrix/subcategories returns 201 or 404 for invalid category
- [ ] GET /api/skill-matrix/skills?subcategoryId= returns 200 and items
- [ ] POST /api/skill-matrix/skills returns 201 or 404 for invalid subcategory
- [ ] Unauthorized (no token / wrong role) returns 401 or 403

### Employee Profile

- [ ] GET /api/skill-matrix/profile/me returns 200 with employeeId and skills array
- [ ] POST /api/skill-matrix/profile/me/skills with skillId and selfAssessedLevel (1–4) returns 201 or 400 (e.g. already on profile)
- [ ] PATCH /api/skill-matrix/profile/me/skills/{employeeSkillId} with selfAssessedLevel returns 204 or 404
- [ ] GET /api/skill-matrix/profile/{employeeId} as Manager returns 200; as Employee (other id) returns 403

### Manager Approval

- [ ] GET /api/skill-matrix/approvals/pending returns 200 with items array
- [ ] PATCH /api/skill-matrix/approvals/{employeeSkillId} with action "approve" and optional managerValidatedLevel/managerNotes returns 204 or 404
- [ ] PATCH with action "reject" and optional managerNotes returns 204 or 404
- [ ] PATCH with invalid action returns 400

### Frontend (Manual / E2E)

- [ ] Skill profile page loads and displays current user skills (p-table)
- [ ] Add skill and update proficiency flows work with PrimeNG forms
- [ ] Manager dashboard shows pending approvals and approve/reject works (p-dialog)
- [ ] Admin taxonomy page allows CRUD for categories, subcategories, skills
- [ ] Role guards: Employee cannot access admin taxonomy; Manager cannot access HR Admin–only actions

## Automation

- Backend contract/integration: `backend/tests/ITP.Api.Tests` (TaxonomyContractsTests, EmployeeProfileFlowTests, ManagerApprovalFlowTests)
- Run: `dotnet test backend/tests/ITP.Api.Tests/ITP.Api.Tests.csproj`

## Sign-off

- [ ] All automated tests pass
- [ ] Manual/E2E checklist completed for MVP flows
- [ ] No [Authorize] bypass; 401/403 as expected for unauthenticated/forbidden requests
