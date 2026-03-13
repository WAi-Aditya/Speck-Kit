# Phase 2 Regression Test Suite: Certifications + Assessments + Notifications (US2)

**Purpose**: Validate certification upload, assessment workflow, and notification lifecycle before User Story 2 closure.  
**Feature**: specs/001-skill-matrix (User Story 2).

## Scope

- Certification upload (multipart) and list — Employee own profile
- Assessment assign / submit / evaluate workflow
- Notification list and read state; scheduler/reminder behaviour
- Role restrictions (Employee for own certifications/assessments/notifications)

## Test Checklist

### Certifications (Contract + API)

- [ ] GET /api/skill-matrix/profile/me/certifications returns 200 and `items` array
- [ ] POST /api/skill-matrix/profile/me/certifications (multipart: file, skillId, title, issuer, issueDate, expiryDate?) returns 201 with `id` and `documentBlobUrl` (or 400 for validation)
- [ ] Validation: issueDate required; if expiryDate provided, expiryDate >= issueDate; file type/size per config
- [ ] Unauthenticated or wrong role returns 401 or 403

### Assessments (Integration + API)

- [ ] GET /api/skill-matrix/assessments/me returns 200 with `items` array
- [ ] POST /api/skill-matrix/assessments/assign with employeeSkillId, assessmentType, dueDate returns 201 with assessment id
- [ ] PATCH /api/skill-matrix/assessments/{id}/submit returns 204 for assigned assessments
- [ ] POST (or PATCH) /api/skill-matrix/assessments/{id}/evaluate with score, passFail, comments returns 200/204
- [ ] Status flow: Assigned (0) → Submitted (1) → Evaluated (2)
- [ ] Unauthorized access returns 401 or 403

### Notifications (Integration + Scheduler)

- [ ] GET /api/skill-matrix/notifications/me returns 200 with `items` array (id, type, title, body, readAt)
- [ ] PATCH /api/skill-matrix/notifications/{id}/read returns 204
- [ ] Notification types: PendingApproval, ExpiringCertification, OverdueAssessment, ManagerValidated, etc.
- [ ] Scheduler/hosted service or Hangfire job runs and creates reminders (e.g. expiring certifications, overdue assessments)
- [ ] Reminder delivery (e.g. in-app list, email) is verifiable in test environment

### Frontend (Manual / E2E)

- [ ] Certification upload component loads and lists my certifications
- [ ] File selection and upload with p-fileUpload; success/error toast or message
- [ ] Assessments screen lists my assessments with type and status labels
- [ ] Submit action for assigned assessments; list refreshes after submit
- [ ] Notification center shows unread/read notifications and mark-as-read works
- [ ] Role guards: only Employee (or Manager for team) can access own certifications/assessments/notifications

## Automation

- Backend contract: `CertificationUploadContractsTests.cs`
- Backend integration: `AssessmentWorkflowTests.cs`, `NotificationSchedulerTests.cs`
- Frontend: `certification-upload.component.spec.ts`, `assessments.component.spec.ts`
- Run backend: `dotnet test backend/tests/ITP.Api.Tests/ITP.Api.Tests.csproj`
- Run frontend: `cd frontend && npm test`

## Sign-off

- [ ] All automated tests for US2 pass
- [ ] Manual/E2E checklist completed for certification and assessment lifecycle
- [ ] Notification scheduler and reminder behaviour verified
- [ ] No [Authorize] bypass; 401/403 as expected
