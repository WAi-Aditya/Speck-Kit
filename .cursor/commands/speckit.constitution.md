# ITP Skill Matrix Module Constitution

## Core Principles

### I. Hierarchical Skill Taxonomy
All skills must be organised in a strict three-level hierarchy: Category → Sub-Category → Skill (e.g. Development → Frontend → React; Cloud → Azure/AWS → Cloud Services; QA → Automation → Manual Testing). No skill may exist without a Sub-Category parent and a Category grandparent. Taxonomy nodes are admin-managed; employees must not create new nodes directly. Taxonomy changes require admin approval before taking effect.

### II. Calibrated Proficiency Model
Every skill assignment must carry exactly one of four proficiency levels: 1-Beginner (requires guidance), 2-Intermediate (works independently), 3-Advanced (handles complex tasks, mentors others), 4-Expert (drives innovation and strategy). Final Rating = Self-Assessment (40%) + Manager Validation (40%) + System-Generated Score (20%). All three inputs must be present before a rating is persisted. Peer feedback is collected as supplementary input but does not alter formula weights. Manager overrides are permitted with documented justification; overrides must be stored and auditable.

### III. Employee-Owned Skill Profile (NON-NEGOTIABLE)
Every employee owns and maintains their personal skill profile. Profiles must support: adding/updating skills at any time, uploading certifications linked to specific skills, viewing proficiency progression over time, and reviewing current levels with recommended next steps. Employees must not be blocked from submitting self-assessments during an open cycle. Certification uploads must store issuer, issue date, expiry date, and linked skill node. Employees must receive notifications when a manager validates or overrides their rating.

### IV. Role-Based Access and Controls
Two distinct control planes must be enforced. Manager controls: approve and validate employee skill submissions, view full team skill matrix, identify team capability gaps. Admin controls: manage skill taxonomy, define skill frameworks and proficiency standards, configure assessment workflows. Managers must only access profiles of their direct or delegated reports; cross-team visibility requires explicit admin grant. Admins must not submit or alter individual proficiency scores directly. All access-control changes must be logged with actor, timestamp, and reason.

### V. Reporting and Actionable Insights
Four standard report types are mandatory: Skill Gap Analysis (missing skills vs. org/project requirements), Team Capability Report (team strengths and development areas), Project Skill Matching (ranked candidates matched to project skill needs), and Organisation Skill Heatmap (visual skill distribution across the org). All reports must be filterable by department, team, skill category, and date range. Reports must not expose personal data beyond name, role, and skill profile. Heatmaps must be exportable to CSV and PDF.

### VI. ITP Platform Integration
The module must integrate natively with four ITP platform touchpoints: Employee Profiles (read/write — sync base data, push ratings), Project Allocation (read — match skills to project requirements), Performance Reviews (read/write — feed ratings into review cycles), Learning & Development (read/write — consume completions, push skill gap signals). All integrations must use internal ITP API contracts; no direct cross-database joins permitted. Breaking contract changes require a minimum two-sprint deprecation window. The module must remain operable in a degraded-integration state; loss of one integration must not take down core workflows.

## Integration Constraints

The Skill Matrix Module is a first-class citizen of the ITP Platform ecosystem, not a standalone tool. All data flows must go through versioned internal API contracts. The module must be independently deployable without requiring coordinated releases across all integrated modules. Skill taxonomy and proficiency standards defined here are the single source of truth; no other ITP module may define conflicting skill classification schemes.

## Workforce Impact Standards

Target outcomes that all delivery decisions must support: ≥ 35% improvement in project staffing speed, ≥ 60% increase in workforce visibility, ≥ 45% improvement in career development tracking, ≥ 80% skill gap identification accuracy. Features or scope changes that demonstrably regress these outcomes require Product Manager sign-off before proceeding. v1 scope excludes: payroll/compensation adjustments based on skill ratings, external talent marketplace integrations, and AI-generated learning content.

## Governance

This constitution supersedes all other project practices and documentation. Amendments affecting the Proficiency Model (Principle II) or Skill Taxonomy (Principle I) require sign-off from both the Product Manager and Engineering Lead. All other amendments require Product Manager approval only. Approved amendments must be applied to this constitution and all dependent templates within one sprint. All PRs and feature reviews must verify compliance with these principles. Complexity must be justified against the workforce impact standards above. Use `.specify/memory/constitution.md` as the authoritative runtime reference during development.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE) | **Last Amended**: 2026-03-12