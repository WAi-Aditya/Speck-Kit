# Code Generation Readiness: Skill Matrix Module

**Purpose**: Identify where specs are sufficient for good, bug-free code generation and where more content is needed. Use this when generating code from spec.md, plan.md, tasks.md, constitution, and data-model/contracts.

---

## Summary

| Artifact | Status | Use for code gen | Gaps addressed |
|----------|--------|------------------|----------------|
| **spec.md** | Good + enhanced | User stories, FRs, edge cases, success criteria | Added validation rules, entity field details, error-handling expectations |
| **plan.md** | Good + filled | Tech stack, backend/frontend tasks, structure | Filled Summary, Constitution Check, Project Structure, Structure Decision |
| **tasks.md** | Good + fixed | File paths, phase order, test-first | Fixed wrong paths; added error-handling/validation notes where needed |
| **data-model.md** | **Added** | Entity fields, types, relationships, validation | Single source of truth for DB/entities — reduces bugs from guesswork |
| **contracts/** | **Added** | API request/response shapes, status codes | Reduces API bugs and mismatches between frontend and backend |
| **constitution.md** | Good | P1–P6 rules, scoring formula, RBAC | No change — already in .specify/memory |
| **tech-stack.md** | Good | .NET, Angular, PrimeNG, theme | No change |

---

## Where It Is Good to Create Code

- **Backend domain entities and EF config**: Use **data-model.md** + **plan.md** (BE-01, BE-02) + **constitution** (P1, P2).
- **Backend APIs (controllers, DTOs)**: Use **contracts/** + **plan.md** (BE-03–BE-08) + **spec.md** (FRs).
- **Frontend feature module, routes, services**: Use **plan.md** (FE-01, FE-02) + **contracts/** for API client shapes.
- **Frontend screens (profile, dashboard, taxonomy, reports)**: Use **spec.md** (user stories, acceptance scenarios) + **plan.md** (FE-03–FE-07) + **tech-stack.md** (PrimeNG, professional/clean theme).
- **Authorization and guards**: Use **constitution** (P4) + **spec.md** (FR-010) + **plan.md** (BE-11, FE-09).
- **Tests**: Use **tasks.md** (test tasks T017–T022, etc.) + **spec.md** (acceptance scenarios, edge cases) + **contracts/** for contract tests.

---

## What Was Added for Excellent, Bug-Free Code Generation

1. **data-model.md** — Entity names, field types, required/optional, max lengths, relationships, and validation rules. Use for: EF entities, configurations, migrations, and service-layer validation.
2. **contracts/** — API contract (taxonomy, profile, approval) with HTTP methods, request/response bodies, status codes, and error shapes. Use for: controller actions, DTOs, and frontend service methods.
3. **spec.md** — New section **Validation & Business Rules** (proficiency levels, taxonomy hierarchy, certification dates, approval states). Entity details expanded with field-level constraints. **Error-handling expectations** (what to return and show for validation/not-found/forbidden).
4. **plan.md** — Summary, Constitution Check (gates from constitution), concrete **Project Structure** (backend/frontend paths), and **Structure Decision** so generators use correct paths.
5. **tasks.md** — Corrected paths (`001-skill-matrix` not `satish-qa-skill-matrix`); T070 quickstart path fixed. Notes on validation and error handling for key implementation tasks.
6. **quickstart.md** — Stub added so references (e.g. T070) resolve; can be expanded by `/speckit.plan` or manually.

---

## Rules for Code Generators

- **Entities and DB**: Implement exactly from **data-model.md**; do not invent fields or types.
- **APIs**: Implement request/response and status codes from **contracts/**; return problem details (RFC 7807) for validation and errors.
- **Business rules**: Enforce **constitution** (P1–P6) and **spec.md** Validation & Business Rules in services, not only in UI.
- **UI**: Follow **tech-stack.md** (Angular, PrimeNG, professional and clean theme); use loading, empty, and error states as in spec/plan.
- **Tests**: Cover happy path, validation failures, and edge cases from **spec.md**; align contract tests with **contracts/**.
