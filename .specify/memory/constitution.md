<!--
SYNC IMPACT REPORT
==================
Version change: N/A → 1.0.0 (initial ratification)

Modified principles: None (initial creation)

Added sections:
  - Project Identity
  - Principles (P1–P6)
  - Governance

Removed sections: None

Templates requiring updates:
  ⚠ .specify/templates/plan-template.md       — pending (not yet initialized)
  ⚠ .specify/templates/spec-template.md       — pending (not yet initialized)
  ⚠ .specify/templates/tasks-template.md      — pending (not yet initialized)

Follow-up TODOs:
  - TODO(RATIFICATION_DATE): Confirm exact project kick-off / sign-off date with sponsor.
  - TODO(TECH_STACK): Confirm frontend framework, backend language, and DB engine
    once architecture decision record is approved.
-->

# Project Constitution

**Project:** ITP Platform — Skill Matrix Module
**Version:** 1.0.0
**Ratification Date:** TODO(RATIFICATION_DATE): Insert ISO date when sponsor signs off
**Last Amended:** 2026-03-12
**Status:** Active

---

## 1. Project Identity

### 1.1 Purpose

The Skill Matrix Module extends the ITP Platform to give the organisation
structured, real-time visibility into employee skills. It replaces ad-hoc,
informal tracking with a centralised repository, a calibrated proficiency
model, and analytics that drive smarter staffing and development decisions.

### 1.2 Problem Being Solved

| Pain Point | Impact |
|---|---|
| No structured employee skill tracking | Expertise is invisible to decision-makers |
| Difficult to identify employee expertise | Manual, slow, error-prone talent searches |
| Inefficient project allocation | Mismatched skills increase rework and cost |
| Limited organisational capability visibility | Planning and hiring are reactive, not strategic |

### 1.3 Goals

1. Design and deliver a Skill Matrix Module fully integrated into the ITP Platform.
2. Provide workforce capability visibility that improves project staffing by ≥ 35 %.
3. Achieve ≥ 80 % skill-gap identification accuracy across the organisation.
4. Support employee career development tracking with measurable proficiency progression.

### 1.4 Out of Scope (v1)

- Payroll or compensation adjustments based on skill ratings.
- External talent marketplace or recruitment integrations.
- AI/ML-generated learning content (analytics and gap identification only).

---

## 2. Principles

### P1 — Hierarchical Skill Taxonomy

**Name:** Hierarchical Skill Taxonomy

All skills MUST be organised in a three-level hierarchy:
`Category → Sub-Category → Skill`.

Canonical examples:

| Category | Sub-Category | Skill |
|---|---|---|
| Development | Frontend | React |
| Cloud | Azure / AWS | Cloud Services |
| QA | Automation | Manual Testing |

**Rules:**
- No skill MAY exist without a Sub-Category parent and a Category grandparent.
- Category and Sub-Category nodes MUST be admin-managed; employees MUST NOT
  create new taxonomy nodes directly.
- Taxonomy changes MUST go through an admin approval workflow before taking effect.

**Rationale:** A flat tag list degrades into inconsistent naming within weeks.
A strict hierarchy enables roll-up analytics (team → department → org) and
powers the project-skill matching engine.

---

### P2 — Calibrated Proficiency Model

**Name:** Calibrated Proficiency Model

Every skill assignment MUST carry exactly one of four proficiency levels:

| Level | Label | Descriptor |
|---|---|---|
| 1 | Beginner | Basic understanding; requires guidance and supervision |
| 2 | Intermediate | Works independently on standard tasks with confidence |
| 3 | Advanced | Handles complex tasks; mentors others effectively |
| 4 | Expert | Subject-matter expert; drives innovation and strategy |

**Proficiency Score Formula:**

```
Final Rating = (Self-Assessment × 0.40)
             + (Manager Validation × 0.40)
             + (System-Generated Score × 0.20)
```

Peer feedback is collected as a supplementary input and MUST be surfaced to
the manager during validation but MUST NOT directly alter the formula weight.

**Rules:**
- All three scoring inputs MUST be present before a final rating is persisted.
- System-generated scores MUST be derived from verifiable artefacts
  (certifications, project contribution history, assessment completions).
- A manager MAY override a final rating with a documented justification; the
  override and justification MUST be stored and auditable.

**Rationale:** A blended score reduces self-rating inflation and manager
recency bias while keeping the process transparent and contestable.

---

### P3 — Employee-Owned Skill Profile

**Name:** Employee-Owned Skill Profile

Every employee MUST own and maintain their personal skill profile. The profile
MUST support:

- Adding or updating skill entries at any time.
- Uploading certifications and credentials linked to specific skills.
- Viewing personal proficiency progression over time.
- Reviewing current proficiency levels and recommended next steps.

**Rules:**
- Employees MUST NOT be blocked from submitting a self-assessment at any time
  during an open assessment cycle.
- Certification uploads MUST be stored with metadata: issuer, issue date,
  expiry date (if applicable), and linked skill node.
- Employees MUST receive a notification when a manager validates or overrides
  their submitted rating.

**Rationale:** Ownership drives accuracy. When employees can see and manage
their own profiles they have an incentive to keep data current, which in turn
keeps org-level analytics trustworthy.

---

### P4 — Role-Based Access and Controls

**Name:** Role-Based Access and Controls

The module MUST enforce two distinct control planes:

**Manager controls:**
- Approve and validate employee skill submissions.
- View the full team skill matrix.
- Identify team capability gaps against project or department requirements.

**Admin controls:**
- Manage and organise skill categories (taxonomy governance).
- Define skill frameworks and proficiency standards.
- Configure assessment workflows and scoring parameters.

**Rules:**
- Managers MUST only access profiles belonging to their direct or delegated
  reports; cross-team visibility requires explicit admin grant.
- Admins MUST NOT be able to submit or alter individual employee proficiency
  scores directly; that responsibility belongs to the calibration workflow.
- All access-control changes MUST be logged with actor, timestamp, and reason.

**Rationale:** Separation of concerns prevents both data contamination
(admins gaming scores) and privacy violations (managers viewing peers'
profiles without authorisation).

---

### P5 — Reporting and Actionable Insights

**Name:** Reporting and Actionable Insights

The module MUST provide four standard report types:

| Report | Description |
|---|---|
| Skill Gap Analysis | Missing skills vs. organisational/project requirements |
| Team Capability Report | Team strengths and development areas |
| Project Skill Matching | Available talent matched to project skill needs |
| Organisation Skill Heatmap | Visual skill distribution across the org |

**Rules:**
- All reports MUST be filterable by department, team, skill category, and date
  range at minimum.
- Project Skill Matching MUST surface ranked candidates, not just binary
  matches.
- Reports MUST NOT expose individual salary or personal data beyond name,
  role, and skill profile.
- Heatmaps MUST be exportable to CSV and PDF.

**Rationale:** Analytics are the primary value driver for leadership and
HR. Without actionable, filterable reports the module becomes a data warehouse
with no business return.

---

### P6 — ITP Platform Integration

**Name:** ITP Platform Integration

The Skill Matrix Module MUST integrate natively with the following ITP
Platform modules:

| Integration Point | Direction | Purpose |
|---|---|---|
| Employee Profiles | Read / Write | Sync base employee data; push skill ratings back |
| Project Allocation | Read | Match skill requirements to available employees |
| Performance Reviews | Read / Write | Feed skill ratings into review cycles |
| Learning & Development | Read / Write | Consume course completions; push skill gap signals |

**Rules:**
- All integrations MUST use the ITP Platform's internal API contracts; no
  direct database cross-joins are permitted.
- Breaking changes to integration contracts MUST be versioned and communicated
  with a minimum two-sprint deprecation window.
- The module MUST remain operable in a degraded-integration state; loss of one
  integration MUST NOT take down the Skill Matrix UI or core workflows.

**Rationale:** The module's long-term value compounds only if it is a
first-class citizen of the ITP ecosystem. Isolation would reduce it to a
standalone spreadsheet replacement.

---

## 3. Governance

### 3.1 Amendment Procedure

1. Any stakeholder MAY propose an amendment by raising a documented change
   request referencing the affected principle(s) and rationale.
2. The Product Manager MUST review the proposal within five business days.
3. Amendments affecting P1 (Taxonomy) or P2 (Proficiency Model) require
   sign-off from both the Product Manager and the Engineering Lead.
4. All other amendments require Product Manager approval only.
5. Approved amendments MUST be applied to this constitution and all dependent
   templates within one sprint of approval.

### 3.2 Versioning Policy

| Change Type | Version Bump | Examples |
|---|---|---|
| Backward-incompatible principle removal or redefinition | MAJOR (X.0.0) | Removing a scoring input from P2; restructuring taxonomy levels in P1 |
| New principle or materially expanded guidance | MINOR (x.Y.0) | Adding a new integration point to P6; introducing a fifth proficiency level |
| Clarification, wording, typo fix | PATCH (x.y.Z) | Correcting a table label; rewording a rule for clarity |

### 3.3 Compliance Review

- The constitution MUST be reviewed at the start of each programme increment
  (or quarterly if increments are not used).
- The Product Manager is the designated Constitution Owner and is accountable
  for review scheduling and enforcement.
- Each review MUST produce one of: (a) no change with a dated confirmation
  comment, (b) a patch/minor/major amendment following 3.1.

### 3.4 Deferred Items

| ID | Field | Notes |
|---|---|---|
| TODO-1 | RATIFICATION_DATE | Insert ISO date (YYYY-MM-DD) when project sponsor formally signs off |
| TODO-2 | TECH_STACK | Confirm frontend framework, backend language, and database engine via ADR |
