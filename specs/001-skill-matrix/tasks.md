# Tasks: Skill Matrix Module Integration in ITP

**GitHub Issues**: [Speck-Kit Issues](https://github.com/WAi-Aditya/Speck-Kit/issues) — To create issues from these tasks, run `specs/001-skill-matrix/create-github-issues.ps1` (requires [GitHub CLI](https://cli.github.com/) and `gh auth login`). Issue data is in `specs/001-skill-matrix/github-issues-from-tasks.json`.

**Input**: Design documents from /specs/001-skill-matrix/ (spec.md, plan.md, data-model.md, contracts/)
**Prerequisites**: plan.md

**Tests**: Included because QA non-negotiables require positive, negative, functional, non-functional, edge-case, integration, and regression coverage before story closure.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Initialize module scaffolding and shared dependencies for backend and frontend.

- [x] T001 Create backend module folders for skill matrix in backend/src/Modules/SkillMatrix/
- [x] T002 Create frontend feature folders in frontend/src/app/features/skill-matrix/
- [x] T003 [P] Add backend package references for Azure Blob and Hangfire in backend/ITP.Api.csproj
- [x] T004 [P] Add frontend dependencies for PrimeNG Chart and PrimeIcons in frontend/package.json
- [x] T005 Configure Swagger grouping for Skill Matrix endpoints in backend/src/Presentation/Swagger/SwaggerConfig.cs
- [x] T006 Configure environment settings for Azure Blob and notification scheduler in backend/src/Infrastructure/Configuration/SkillMatrixOptions.cs

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Complete shared core and security prerequisites that block all user story implementation.

**CRITICAL**: No user story work can begin until this phase is complete.

- [x] T007 Implement domain enums SkillLevel and ValidationStatus in backend/src/Modules/SkillMatrix/Domain/Enums/SkillEnums.cs
- [x] T008 [P] Implement base audit entity for Skill Matrix entities in backend/src/Modules/SkillMatrix/Domain/Common/AuditableEntity.cs
- [x] T009 Implement core entities SkillCategory, SkillSubCategory, Skill, SkillLevelCriteria in backend/src/Modules/SkillMatrix/Domain/Entities/TaxonomyEntities.cs
- [x] T010 [P] Implement core entities EmployeeSkill and EmployeeSkillProject in backend/src/Modules/SkillMatrix/Domain/Entities/EmployeeSkillEntities.cs
- [x] T011 [P] Implement core entities Certification, Assessment, AssessmentResult, Notification in backend/src/Modules/SkillMatrix/Domain/Entities/AssessmentEntities.cs
- [x] T012 Configure EF Core mappings and constraints for all Skill Matrix entities in backend/src/Modules/SkillMatrix/Infrastructure/Persistence/Configurations/
- [x] T013 Create EF Core migration for Skill Matrix schema in backend/src/Infrastructure/Persistence/Migrations/
- [x] T014 Register Skill Matrix DbSets and module services in backend/src/Infrastructure/Persistence/ITPDbContext.cs
- [x] T015 Implement role-based authorization policies Employee/Manager/HRAdmin/Leadership in backend/src/Presentation/Auth/SkillMatrixPolicies.cs
- [x] T016 [P] Add Angular role guard and permission directive for skill matrix routes in frontend/src/app/core/auth/skill-matrix-permissions.ts

**Checkpoint**: Foundation ready. User story phases can proceed.

---

## Phase 3: User Story 1 - MVP Taxonomy + Self Assessment + Manager Approval (Priority: P1)

**Goal**: Deliver the MVP workflow for taxonomy management, employee skill updates, and manager approval.

**Independent Test**: HR Admin can manage taxonomy, Employee can submit/update skills, Manager can approve/reject, and role restrictions are enforced end-to-end.

### Tests for User Story 1

- [x] T017 [P] [US1] Add taxonomy CRUD contract tests in backend/tests/Contract/SkillMatrix/TaxonomyContractsTests.cs
- [x] T018 [P] [US1] Add employee profile API integration tests in backend/tests/Integration/SkillMatrix/EmployeeProfileFlowTests.cs
- [x] T019 [P] [US1] Add manager approval API integration tests in backend/tests/Integration/SkillMatrix/ManagerApprovalFlowTests.cs
- [x] T020 [P] [US1] Add frontend component tests for skill profile page in frontend/src/app/features/skill-matrix/skill-profile/skill-profile.component.spec.ts
- [x] T021 [P] [US1] Add frontend component tests for taxonomy admin page in frontend/src/app/features/skill-matrix/admin-taxonomy/admin-taxonomy.component.spec.ts
- [x] T022 [P] [US1] Add regression test suite for MVP flow in tests/regression/skill-matrix/mvp-regression.md

### Implementation for User Story 1

- [x] T023 [US1] Implement taxonomy application services in backend/src/Modules/SkillMatrix/Application/Taxonomy/TaxonomyService.cs
- [x] T024 [US1] Implement taxonomy CRUD API controller (Admin only) in backend/src/Modules/SkillMatrix/Presentation/Controllers/SkillTaxonomyController.cs
- [x] T025 [US1] Implement employee skill profile service in backend/src/Modules/SkillMatrix/Application/Profile/EmployeeSkillProfileService.cs
- [x] T026 [US1] Implement employee skill profile API endpoints in backend/src/Modules/SkillMatrix/Presentation/Controllers/EmployeeSkillProfileController.cs
- [x] T027 [US1] Implement manager approval service and workflow rules in backend/src/Modules/SkillMatrix/Application/Approval/ManagerApprovalService.cs
- [x] T028 [US1] Implement manager approval PATCH endpoint in backend/src/Modules/SkillMatrix/Presentation/Controllers/ManagerApprovalController.cs
- [x] T029 [US1] Create Angular lazy-loaded feature routing for MVP pages in frontend/src/app/features/skill-matrix/skill-matrix.routes.ts
- [x] T030 [US1] Implement Angular SkillService API client in frontend/src/app/features/skill-matrix/data-access/skill.service.ts
- [x] T031 [US1] Build employee skill profile page with p-table and edit forms in frontend/src/app/features/skill-matrix/skill-profile/
- [x] T032 [US1] Build manager dashboard approval page with p-table and p-dialog in frontend/src/app/features/skill-matrix/manager-dashboard/
- [x] T033 [US1] Build HR admin taxonomy manager page with PrimeNG CRUD forms in frontend/src/app/features/skill-matrix/admin-taxonomy/
- [x] T034 [US1] Wire role guards and conditional UI actions for MVP screens in frontend/src/app/features/skill-matrix/

**Checkpoint**: User Story 1 is independently functional and demo-ready.

---

## Phase 4: User Story 2 - Certifications + Assessments + Notifications (Priority: P2)

**Goal**: Add certification file upload, structured assessments, and reminder notifications.

**Independent Test**: Employee uploads certification to Azure Blob, assessor lifecycle works, and due reminders are generated and delivered.

### Tests for User Story 2

- [x] T035 [P] [US2] Add certification upload contract tests (multipart) in backend/tests/Contract/SkillMatrix/CertificationUploadContractsTests.cs
- [x] T036 [P] [US2] Add assessment assign/submit/evaluate integration tests in backend/tests/Integration/SkillMatrix/AssessmentWorkflowTests.cs
- [x] T037 [P] [US2] Add notification scheduler integration tests in backend/tests/Integration/SkillMatrix/NotificationSchedulerTests.cs
- [x] T038 [P] [US2] Add frontend tests for certification upload component in frontend/src/app/features/skill-matrix/skill-profile/certification-upload.component.spec.ts
- [x] T039 [P] [US2] Add frontend tests for assessment workflow screens in frontend/src/app/features/skill-matrix/assessments/assessments.component.spec.ts
- [x] T040 [P] [US2] Add regression tests for certification and assessment lifecycle in tests/regression/skill-matrix/phase2-regression.md

### Implementation for User Story 2

- [ ] T041 [US2] Implement Azure Blob storage adapter for certifications in backend/src/Modules/SkillMatrix/Infrastructure/Storage/AzureBlobCertificationStorage.cs
- [ ] T042 [US2] Implement certification upload service and metadata persistence in backend/src/Modules/SkillMatrix/Application/Certification/CertificationService.cs
- [ ] T043 [US2] Implement certification multipart upload API in backend/src/Modules/SkillMatrix/Presentation/Controllers/CertificationController.cs
- [ ] T044 [US2] Implement assessment domain services (assign, submit, evaluate) in backend/src/Modules/SkillMatrix/Application/Assessment/AssessmentService.cs
- [ ] T045 [US2] Implement assessment endpoints in backend/src/Modules/SkillMatrix/Presentation/Controllers/AssessmentController.cs
- [ ] T046 [US2] Implement notification trigger handlers and email sender integration in backend/src/Modules/SkillMatrix/Application/Notifications/NotificationService.cs
- [ ] T047 [US2] Implement scheduled reminder worker using hosted service or Hangfire in backend/src/Modules/SkillMatrix/Infrastructure/Jobs/SkillMatrixReminderJob.cs
- [ ] T048 [US2] Extend Angular SkillService for certification and assessment endpoints in frontend/src/app/features/skill-matrix/data-access/skill.service.ts
- [ ] T049 [US2] Build certification upload UI with p-fileUpload and p-toast in frontend/src/app/features/skill-matrix/skill-profile/certification-upload/
- [ ] T050 [US2] Build assessment assign/submit/evaluate UI in frontend/src/app/features/skill-matrix/assessments/
- [ ] T051 [US2] Build notification center UI in frontend/src/app/features/skill-matrix/notifications/

**Checkpoint**: User Stories 1 and 2 both operate independently and pass regression.

---

## Phase 5: User Story 3 - Reports + Heatmap + Project Matching + Integrations (Priority: P3)

**Goal**: Deliver decision-support reports, organization heatmap, project matching, and cross-module integration points.

**Independent Test**: Leadership can view heatmap/reporting metrics, project matching recommendations work, and data sync is visible in dependent ITP modules.

### Tests for User Story 3

- [ ] T052 [P] [US3] Add reporting and heatmap contract tests in backend/tests/Contract/SkillMatrix/ReportingContractsTests.cs
- [ ] T053 [P] [US3] Add project matching integration tests in backend/tests/Integration/SkillMatrix/ProjectMatchingTests.cs
- [ ] T054 [P] [US3] Add cross-module integration tests for profile and allocation hooks in backend/tests/Integration/SkillMatrix/ModuleIntegrationTests.cs
- [ ] T055 [P] [US3] Add frontend tests for reports dashboard in frontend/src/app/features/skill-matrix/reports/reports.component.spec.ts
- [ ] T056 [P] [US3] Add frontend tests for project matching screen in frontend/src/app/features/skill-matrix/project-matching/project-matching.component.spec.ts
- [ ] T057 [P] [US3] Add regression tests for reports and matching in tests/regression/skill-matrix/phase3-regression.md

### Implementation for User Story 3

- [ ] T058 [US3] Implement reporting and gap-analysis services in backend/src/Modules/SkillMatrix/Application/Reporting/ReportingService.cs
- [ ] T059 [US3] Implement reporting endpoints for gap analysis, heatmap, team report, and matching in backend/src/Modules/SkillMatrix/Presentation/Controllers/ReportingController.cs
- [ ] T060 [US3] Implement Employee Profile integration adapter in backend/src/Modules/SkillMatrix/Integration/EmployeeProfileIntegrationService.cs
- [ ] T061 [US3] Implement Project Allocation integration adapter in backend/src/Modules/SkillMatrix/Integration/ProjectAllocationIntegrationService.cs
- [ ] T062 [US3] Implement Performance Review and L&D integration adapters in backend/src/Modules/SkillMatrix/Integration/PerformanceAndLearningIntegrationService.cs
- [ ] T063 [US3] Build reports dashboard with p-chart heatmap/trends in frontend/src/app/features/skill-matrix/reports/
- [ ] T064 [US3] Build project matching screen and ranking table in frontend/src/app/features/skill-matrix/project-matching/
- [ ] T065 [US3] Add leadership read-only analytics route and filters in frontend/src/app/features/skill-matrix/reports/reports.routes.ts

**Checkpoint**: All user stories are independently functional.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final quality hardening, documentation, and SLA validation.

- [ ] T066 [P] Add API documentation examples for all Skill Matrix endpoints in backend/src/Modules/SkillMatrix/Presentation/Swagger/SkillMatrixSwaggerExamples.cs
- [ ] T067 Add centralized error mapping and validation problem details in backend/src/Modules/SkillMatrix/Presentation/Filters/SkillMatrixExceptionFilter.cs
- [ ] T068 [P] Add backend performance test scenarios for skill APIs against SLA in tests/performance/skill-matrix/backend-sla.k6.js
- [ ] T069 [P] Add frontend page-load benchmark checks for key screens in tests/performance/skill-matrix/frontend-sla.md
- [ ] T070 Run end-to-end QA checklist and release sign-off evidence in specs/001-skill-matrix/quickstart.md

---

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1) has no dependencies.
- Foundational (Phase 2) depends on Setup and blocks all user stories.
- User Story phases (Phase 3-5) depend on Foundational completion.
- Polish (Phase 6) depends on completion of selected user stories.

### User Story Dependencies

- US1 (P1) starts first and provides MVP capability.
- US2 (P2) depends on US1 authentication/identity and profile baseline APIs.
- US3 (P3) depends on US1 and US2 data completeness for reporting and matching.

### Within Each User Story

- Tests first, then models/services, then controllers/UI integration.
- Backend APIs before frontend page wiring for the same feature.
- Regression tests must pass before story closure.

### Parallel Opportunities

- T003 and T004 can run in parallel.
- T010 and T011 can run in parallel after T009.
- T017 to T022 can run in parallel in US1 test stream.
- T035 to T040 can run in parallel in US2 test stream.
- T052 to T057 can run in parallel in US3 test stream.
- T068 and T069 can run in parallel in Polish.

---

## Parallel Example: User Story 1

- Run T017, T018, T019 in parallel as API test tasks.
- Run T020 and T021 in parallel as frontend component test tasks.
- Run T031, T032, and T033 in parallel after T029 and T030 are complete.

---

## Implementation Strategy

### MVP First (US1)

1. Complete Phase 1 and Phase 2.
2. Complete US1 implementation and tests.
3. Validate MVP independently and demo HR Admin/Employee/Manager workflow.

### Incremental Delivery

1. Deliver US1 as MVP.
2. Add US2 certifications, assessments, notifications.
3. Add US3 reporting, matching, and ITP integrations.
4. Run phase regression before moving to next phase.

### Parallel Team Strategy

1. Team A handles backend APIs/services by story.
2. Team B handles frontend pages/components by story.
3. Team C handles QA automation and regression packs per story.

---

## Notes

- Every task includes explicit file paths for execution.
- All user-story tasks are tagged with [US1], [US2], or [US3].
- QA non-negotiables are represented with contract, integration, regression, and SLA validation tasks.
- **Code generation**: Use **data-model.md** for entity fields, types, and validation rules; use **contracts/** for API request/response shapes and status codes. Implement validation and error handling per **spec.md** (Validation & Business Rules, Error-Handling Expectations) to reduce bugs. See **CODE_GENERATION_READINESS.md** for full guidance.
