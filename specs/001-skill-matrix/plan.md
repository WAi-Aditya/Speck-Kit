# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]
**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

[Extract from feature spec: primary requirement + technical approach from research]

## Technical Context

**Backend**: .NET Core (ASP.NET Core 8) with EF Core 8  
**Frontend / UI**: Angular 17+ with PrimeNG 17 for components and styling  
**Theme / Design**: Professional and clean — clear hierarchy, ample whitespace, readable typography, consistent spacing and colors  
**Primary Dependencies**: ASP.NET Core 8, EF Core 8, SQL Server; Angular 17, PrimeNG 17, Bootstrap 5  
**Storage**: SQL Server  
**Testing**: xUnit/NUnit + WebApplicationFactory (backend); Jasmine/Karma (frontend)  
**Target Platform**: Windows/Linux server; modern browsers  
**Project Type**: Web application (backend API + frontend SPA)  
**Performance Goals**: Report generation under 5s for teams up to 200; API response within SLA  
**Constraints**: Role-based access; integration via ITP internal APIs only  
**Scale/Scope**: Organisation-wide skill matrix; multiple feature modules (profile, taxonomy, reports, matching)

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

[Gates determined based on constitution file]

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
# [REMOVE IF UNUSED] Option 1: Single project (DEFAULT)
src/
├── models/
├── services/
├── cli/
└── lib/

tests/
├── contract/
├── integration/
└── unit/

# [REMOVE IF UNUSED] Option 2: Web application (when "frontend" + "backend" detected)
backend/
├── src/
│   ├── models/
│   ├── services/
│   └── api/
└── tests/

frontend/
├── src/
│   ├── components/
│   ├── pages/
│   └── services/
└── tests/

# [REMOVE IF UNUSED] Option 3: Mobile + API (when "iOS/Android" detected)
api/
└── [same as backend above]

ios/ or android/
└── [platform-specific structure: feature modules, UI flows, platform tests]
```

**Structure Decision**: [Document the selected structure and reference the real
directories captured above]

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |

## Implementation Tasks

### Backend Tasks (ASP.NET Core 8 + EF Core 8 + SQL Server)

- [ ] BE-01: Add Skill Matrix domain models and enums in the existing ITP backend.
  - Tables/entities: SkillCategory, SkillSubCategory, Skill, SkillLevelCriteria, EmployeeSkill, EmployeeSkillProject, Certification, Assessment, AssessmentResult, Notification.
  - Enums: SkillLevel, ValidationStatus, NotificationType, AssessmentStatus.
  - Add audit fields: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy.

- [ ] BE-02: Create EF Core configurations and migrations.
  - Define PK/FK constraints and indexes for employee, skill, project, and assessment relations.
  - Add seed data for baseline skill levels and validation statuses.

- [ ] BE-03: Implement Skill Taxonomy CRUD APIs (HR Admin only).
  - Endpoints: category, subcategory, skill, and skill-level criteria CRUD.
  - Apply role policies and validation rules for duplicate/invalid taxonomy operations.

- [ ] BE-04: Implement Employee Skill Profile APIs.
  - Endpoints: get employee profile, add skill, update skill proficiency, add project evidence.
  - Enforce ownership rules for employee updates.

- [ ] BE-05: Implement Certification upload and persistence.
  - Multipart upload endpoint.
  - Azure Blob Storage upload service.
  - Persist blob URL and metadata in Certification table.

- [ ] BE-06: Implement manager approval workflow.
  - PATCH endpoint for approve/reject employee skills/certifications.
  - Store validation status, reviewer notes, and audit trail.

- [ ] BE-07: Implement assessment APIs.
  - Assign assessment, submit assessment, evaluate results.
  - Persist scores and pass/fail decisions in AssessmentResult.

- [ ] BE-08: Implement reporting APIs.
  - Gap analysis endpoint.
  - Heatmap data endpoint.
  - Team competency report endpoint.
  - Project skill matching endpoint.

- [ ] BE-09: Implement notifications.
  - Trigger events: pending approvals, expiring certifications, overdue assessments.
  - Delivery channels: in-app notifications + email.
  - Scheduled reminders via .NET Hosted Service or Hangfire.

- [ ] BE-10: Integrate with existing ITP modules.
  - Employee Profile integration for skill summary card.
  - Project Allocation integration for skill matching.
  - Performance Review and L&D integration points.

- [ ] BE-11: Security, documentation, and tests.
  - Role-based authorization policies for Employee, Manager, HR Admin, Leadership.
  - Swagger docs for all endpoints.
  - Unit and integration tests for API, service layer, and RBAC behavior.

### Frontend Tasks (Angular 17 + PrimeNG 17 + Bootstrap 5)

- [ ] FE-01: Create lazy-loaded Skill Matrix feature module.
  - Route prefix under existing ITP Angular app.
  - Child routes: skill-profile, manager-dashboard, admin-taxonomy, reports, project-matching.

- [ ] FE-02: Create shared types and API clients.
  - Models matching backend DTOs and enums.
  - Services: SkillService, AssessmentService, ReportService, NotificationService.

- [ ] FE-03: Build Employee Skill Profile screen.
  - Use p-table for skill list.
  - Use p-dropdown/p-input components for proficiency editing.
  - Use p-fileUpload for certification upload.
  - Use p-toast for success/error feedback.

- [ ] FE-04: Build Manager Team Skill Dashboard.
  - Team skill table with filters.
  - Approval queue with p-dialog for review and comments.
  - Inline status chips for pending/approved/rejected.

- [ ] FE-05: Build HR Admin Taxonomy Manager.
  - CRUD screens for categories/subcategories/skills.
  - Form validation and duplicate protection messages.

- [ ] FE-06: Build Reports module.
  - Heatmap and trend charts using p-chart (Chart.js wrapper).
  - Team and organization-level competency views.

- [ ] FE-07: Build Project Skill Matching screen.
  - Select project and required skills.
  - Display ranked employees with match percentages.

- [ ] FE-08: Implement notification center UI.
  - In-app notification list with unread/read status.
  - Employee and manager reminder indicators.

- [ ] FE-09: Add role-based route guards and UI permissions.
  - Restrict edit/approval/admin actions by role.
  - Hide unauthorized actions while preserving read-only access where applicable.

- [ ] FE-10: Add responsive behavior and UX consistency.
  - Bootstrap grid for desktop/tablet/mobile breakpoints.
  - Loading states, empty states, and retry flows.

- [ ] FE-11: Frontend testing and quality gates.
  - Unit tests for services and components.
  - Integration tests for key role-based flows.
  - Validate SLA-focused scenarios for page load and API response surfaces.

### Delivery Phases

- [ ] Phase 1 (MVP): BE-01 to BE-04, BE-06, FE-01 to FE-05, FE-09.
- [ ] Phase 2: BE-05, BE-07, BE-09, FE-03 upload enhancements, FE-08.
- [ ] Phase 3: BE-08, BE-10, FE-06, FE-07, FE-10, FE-11, BE-11 final hardening.
