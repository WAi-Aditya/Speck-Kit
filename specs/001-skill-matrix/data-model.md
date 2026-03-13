# Data Model: Skill Matrix Module

**Input**: spec.md, constitution (P1–P2). **Used for**: EF entities, configurations, migrations, service-layer validation.

---

## Conventions

- **PK**: Primary key. **FK**: Foreign key. All entities that are audited include: `CreatedAt` (UTC), `UpdatedAt` (UTC), `CreatedBy`, `UpdatedBy` (user/actor id).
- **Required** = NOT NULL. **Optional** = nullable unless stated otherwise.
- Validation rules here MUST be enforced in application layer and, where possible, in DB constraints.

---

## Taxonomy (P1: Hierarchy)

### SkillCategory

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| Name | string | Yes | Max 200, unique |
| Description | string | No | Max 1000 |
| IsActive | bool | Yes | Default true |
| SortOrder | int | No | For display order |
| CreatedAt, UpdatedAt, CreatedBy, UpdatedBy | (audit) | Yes | |

- **Rules**: No skill or subcategory without a category. Deactivate (IsActive = false) instead of delete when in use.

### SkillSubCategory

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| SkillCategoryId | Guid | Yes | FK → SkillCategory |
| Name | string | Yes | Max 200; unique within category |
| Description | string | No | Max 1000 |
| IsActive | bool | Yes | Default true |
| SortOrder | int | No | |
| (audit) | | Yes | |

- **Rules**: No skill without a subcategory. Category must exist and be active for subcategory to be valid.

### Skill

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| SkillSubCategoryId | Guid | Yes | FK → SkillSubCategory |
| Name | string | Yes | Max 200; unique within subcategory |
| Description | string | No | Max 1000 |
| IsActive | bool | Yes | Default true |
| SortOrder | int | No | |
| (audit) | | Yes | |

- **Rules**: Must have SubCategory parent and Category grandparent (enforce via FK). Employees cannot create; admin only.

### SkillLevelCriteria

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| SkillLevel | int | Yes | 1=Beginner, 2=Intermediate, 3=Advanced, 4=Expert |
| Label | string | Yes | Max 100 |
| Description | string | No | Max 500 |
| MinScore | decimal? | No | 0–1 if used for thresholds |
| (audit) | | Yes | |

---

## Employee Skills (P2, P3)

### EmployeeSkill

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| EmployeeId | Guid | Yes | FK → Employee (ITP identity) |
| SkillId | Guid | Yes | FK → Skill |
| SelfAssessedLevel | int? | No | 1–4 (SkillLevel enum) |
| ManagerValidatedLevel | int? | No | 1–4 |
| SystemGeneratedScore | decimal? | No | 0–1 |
| FinalRating | decimal? | No | Computed: 0.4×Self + 0.4×Manager + 0.2×System when all present |
| ValidationStatus | int | Yes | enum: Pending, Approved, Rejected |
| ManagerNotes | string | No | Max 2000 |
| ValidatedAt | DateTimeOffset? | No | When manager approved/rejected |
| ValidatedBy | Guid? | No | Manager who validated |
| (audit) | | Yes | |

- **Rules**: One row per employee per skill. FinalRating persisted only when all three inputs present (constitution P2). Manager override with justification stored in ManagerNotes.

### EmployeeSkillProject

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| EmployeeSkillId | Guid | Yes | FK → EmployeeSkill |
| ProjectId | Guid | Yes | FK → Project (ITP) |
| RoleOrContribution | string | No | Max 500 |
| StartDate | DateTimeOffset? | No | |
| EndDate | DateTimeOffset? | No | EndDate >= StartDate if both set |
| (audit) | | Yes | |

---

## Certifications (P3)

### Certification

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| EmployeeId | Guid | Yes | FK → Employee |
| SkillId | Guid | Yes | FK → Skill |
| Title | string | Yes | Max 300 |
| Issuer | string | Yes | Max 300 |
| IssueDate | DateTimeOffset | Yes | |
| ExpiryDate | DateTimeOffset? | No | If set, ExpiryDate >= IssueDate |
| DocumentBlobUrl | string | No | Max 2000 (Azure Blob URL) |
| (audit) | | Yes | |

- **Rules**: Certification linked to one skill. ExpiryDate optional but when provided must be >= IssueDate.

---

## Assessments (P2)

### Assessment

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| EmployeeSkillId | Guid | Yes | FK → EmployeeSkill |
| AssessmentType | int | Yes | enum: Self, Manager, Peer, System |
| Status | int | Yes | enum: Assigned, Submitted, Evaluated |
| DueDate | DateTimeOffset? | No | |
| SubmittedAt | DateTimeOffset? | No | |
| (audit) | | Yes | |

### AssessmentResult

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| AssessmentId | Guid | Yes | FK → Assessment |
| Score | decimal | Yes | 0–1 or 1–4 depending on schema; consistent per type |
| PassFail | bool? | No | If applicable |
| Comments | string | No | Max 2000 |
| EvaluatedAt | DateTimeOffset? | No | |
| (audit) | | Yes | |

---

## Notifications

### Notification

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| Id | Guid | Yes | PK |
| EmployeeId | Guid | Yes | FK → Employee |
| Type | int | Yes | enum: PendingApproval, ExpiringCertification, OverdueAssessment, ManagerValidated, etc. |
| Title | string | Yes | Max 300 |
| Body | string | No | Max 2000 |
| ReadAt | DateTimeOffset? | No | |
| RelatedEntityId | Guid? | No | e.g. EmployeeSkillId, CertificationId |
| (audit) | | Yes | |

---

## Indexes (recommended)

- SkillSubCategory: IX_SkillSubCategory_SkillCategoryId
- Skill: IX_Skill_SkillSubCategoryId
- EmployeeSkill: IX_EmployeeSkill_EmployeeId, IX_EmployeeSkill_SkillId, composite (EmployeeId, SkillId) unique
- Certification: IX_Certification_EmployeeId, IX_Certification_SkillId
- Assessment: IX_Assessment_EmployeeSkillId
- Notification: IX_Notification_EmployeeId_ReadAt

---

## Enums (backend)

- **SkillLevel**: Beginner = 1, Intermediate = 2, Advanced = 3, Expert = 4
- **ValidationStatus**: Pending = 0, Approved = 1, Rejected = 2
- **AssessmentType**: Self = 0, Manager = 1, Peer = 2, System = 3
- **AssessmentStatus**: Assigned = 0, Submitted = 1, Evaluated = 2
- **NotificationType**: PendingApproval, ExpiringCertification, OverdueAssessment, ManagerValidated, …
