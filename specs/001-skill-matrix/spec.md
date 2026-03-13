# Feature Specification: Skill Matrix Module

**Feature Branch**: `001-skill-matrix`  
**Created**: 2026-03-13  
**Status**: Draft  
**Input**: User description: "Skill Matrix module: capture, maintain, and analyze employee skill data in the ITP platform"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Add and Track Skills (Priority: P1)

Employees can add and maintain their skills so that their profile reflects their current capabilities and development goals.

**Why this priority**: This is the core value of the module; without it users cannot capture or update their skill data.

**Independent Test**: An employee logs in, adds a new skill with a proficiency level, and sees it immediately reflected on their profile.

**Acceptance Scenarios**:

1. **Given** an authenticated employee with an empty skill profile, **When** they add a skill from the taxonomy and select a proficiency level, **Then** the skill appears on their profile with the selected level.
2. **Given** an employee with existing skills, **When** they update the proficiency level for a skill, **Then** the updated level is saved and shown on their profile.

---

### User Story 2 - Manager Validation and Approval (Priority: P1)

Managers review and approve skill submissions to ensure skill data is accurate and aligned with business needs.

**Why this priority**: Validation is required to keep the matrix credible and useful for planning.

**Independent Test**: A manager views pending skill submissions, approves one, and the skill becomes part of the employee's approved skill profile.

**Acceptance Scenarios**:

1. **Given** an employee has submitted a new skill or updated proficiency, **When** a manager reviews pending approvals, **Then** the manager can approve or reject the change and add comments.
2. **Given** a manager approves a skill, **When** the employee views their profile, **Then** the skill is marked as manager-approved.

---

### User Story 3 - Skill Reporting and Insights (Priority: P2)

Managers and stakeholders can view reports that surface skill gaps, distribution, and alignment with project needs.

**Why this priority**: Reporting enables data-driven decisions for staffing, training, and project allocation.

**Independent Test**: A manager generates a skill gap report and sees skill categories with low overall proficiency flagged.

**Acceptance Scenarios**:

1. **Given** skill data exists for a team, **When** a manager requests a skill gap analysis report, **Then** the system displays skills where proficiency levels fall below expected benchmarks.
2. **Given** a manager requests a team skill distribution report, **When** the report is generated, **Then** it shows counts of employees per proficiency level per skill category.

---

### Edge Cases

- What happens when an employee attempts to add a skill that is not in the taxonomy? (expected: the system should allow a request for a new skill but not accept it until approved).
- How does the system handle conflicting assessments (e.g., self-assessment is Expert but manager rates Beginner)? (expected: the system retains all inputs and calculates a clear final rating with traceability).
- What happens if a manager is removed from the system while they have pending approvals? (expected: pending approvals are reassigned to another manager or flagged for admin intervention).

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow employees to browse the skill taxonomy (categories, subcategories, skills) and add skills to their profile.
- **FR-002**: The system MUST enable employees to set and update their proficiency level for each skill (Beginner, Intermediate, Advanced, Expert).
- **FR-003**: The system MUST allow employees to upload certifications and link them to specific skills.
- **FR-004**: The system MUST provide a manager approval workflow for newly added skills and proficiency changes.
- **FR-005**: The system MUST store all assessment inputs (self, manager, peer, system-generated) and calculate a final skill score using a configurable weighted average.
- **FR-006**: The system MUST provide reporting views for skill gap analysis, team skill distribution, and project skill matching.
- **FR-007**: The system MUST integrate skill data with the employee profile module, project allocation system, performance review system, and learning & development.
- **FR-008**: The system MUST allow managers/admins to manage the skill taxonomy (add/edit/deactivate categories, subcategories, and skills).
- **FR-009**: The system MUST allow employees to request new skills be added to the taxonomy when they are not available.
- **FR-010**: The system MUST ensure that only authorized users (employees, managers, admins) can view or modify relevant skill data based on their role.

### Key Entities *(include if feature involves data)*

- **Skill**: Competency with name (required, max 200), description (optional), category, subcategory, status (active/inactive). Must have SubCategory parent and Category grandparent. See data-model.md for full fields.
- **SkillEntry (EmployeeSkill)**: Employee's instance of a skill: proficiency level (1–4), assessment history, approval status (Pending/Approved/Rejected). Final rating computed when self, manager, and system scores present; manager may override with justification stored.
- **Assessment**: Rating event with type (self, manager, peer, system), score, date, optional comments. Status: Assigned, Submitted, Evaluated.
- **Certification**: Credential linked to one skill: title, issuer, issue date (required), expiry date (optional; if set must be ≥ issue date), document reference. See data-model.md.
- **Report**: Generated insights (gap analysis, distribution, matching) with filters (department, team, category, date range) and aggregations.

### Validation & Business Rules

- **Proficiency levels**: Exactly four levels — Beginner (1), Intermediate (2), Advanced (3), Expert (4). Any API accepting a level MUST reject values outside 1–4 with a clear validation error.
- **Taxonomy hierarchy**: No skill without a SubCategory; no SubCategory without a Category. Create/update MUST validate parent exists and is active. Deactivate (soft delete) instead of hard delete when entity is in use.
- **Certification dates**: IssueDate required. If ExpiryDate is provided, ExpiryDate MUST be ≥ IssueDate; otherwise return validation error.
- **Approval workflow**: EmployeeSkill can be Pending, Approved, or Rejected. Only managers (for their reports) can set Approved/Rejected; manager override of final rating MUST store justification (e.g. in ManagerNotes).
- **Final rating (constitution P2)**: Persist FinalRating only when SelfAssessedLevel, ManagerValidatedLevel, and SystemGeneratedScore are all present. Formula: 0.4×Self + 0.4×Manager + 0.2×System. Peer feedback is informational only, not in formula.
- **Ownership**: Employee can create/update only their own profile and certifications. Manager can view team and approve/reject only for direct or delegated reports. HR Admin can manage taxonomy only, not individual proficiency scores.

### Error-Handling Expectations

- **Validation failures** (e.g. invalid level, duplicate skill, expiry &lt; issue date): Return 400 with a structured body (e.g. RFC 7807) listing field errors so the UI can show messages next to fields.
- **Not found** (e.g. skill id, employee id): Return 404; UI shows a clear "not found" message, no crash.
- **Forbidden** (valid user, insufficient role or scope): Return 403; UI hides or disables the action and does not expose other users' data.
- **Conflict** (e.g. duplicate skill on profile, delete category in use): Return 409 with a clear message; UI shows retry or change input.
- **Server errors**: Return 500; UI shows a generic error and optional retry; do not leak stack traces to client.

## Dependencies & Assumptions

- The organization maintains a single source of truth for employee identity and reporting structure (e.g., employee IDs, manager relationships).
- The platform supports role-based access control so that employees, managers, and admins can be authorized appropriately.
- Reports are generated from the same skill data store used for employee profiles, ensuring consistency.
- Skill taxonomy changes (add/edit/deactivate) are visible to employees within 24 hours.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 90% of employees can add a new skill and see it reflected in their profile within 2 minutes.
- **SC-002**: Managers can approve or reject skill submissions within one business day for 90% of cases.
- **SC-003**: 95% of generated reports (skill gap, distribution, matching) render within 5 seconds for teams up to 200 employees.
- **SC-004**: The system reduces manual skill tracking (spreadsheets, email) by at least 50% as measured in stakeholder surveys.
- **SC-005**: At least 80% of skill updates have a clear audit trail showing who made the change and when.
