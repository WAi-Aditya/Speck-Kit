# Skill Matrix — Quickstart & QA

**Purpose**: Run and test the Skill Matrix module; capture release sign-off evidence. Referenced by T070.

---

## Prerequisites

- Backend: .NET 8 SDK; SQL Server (or configured connection).
- Frontend: Node 18+; npm or yarn.
- Environment: Azure Blob (for certifications) and notification/scheduler config as per plan.

---

## Run Backend

```bash
cd backend
dotnet restore
dotnet run
```

- API base: as configured (e.g. https://localhost:7xxx).  
- Swagger: `/swagger` (Skill Matrix endpoints grouped).

---

## Run Frontend

```bash
cd frontend
npm install
npm start
```

- App URL as configured. Navigate to Skill Matrix feature (e.g. `/skill-matrix` or route from ITP shell).

---

## QA Checklist (release sign-off)

- [ ] HR Admin can create category → subcategory → skill and see in taxonomy list.
- [ ] Employee can add skill to profile with proficiency level and see it reflected.
- [ ] Manager sees pending approvals, approves/rejects with optional notes; employee sees status update.
- [ ] Employee can upload certification (file + metadata); expiry ≥ issue date enforced.
- [ ] Reports: gap analysis, team distribution, heatmap return within SLA (e.g. under 5s for 200 employees).
- [ ] Project matching returns ranked candidates; no unauthorized data exposed.
- [ ] Role guards: Employee cannot access admin taxonomy; Manager cannot edit other teams’ profiles.
- [ ] Validation errors return 400 with field-level messages; 403/404 handled without crash.

---

## Notes

- Expand with environment-specific steps and screenshots for release evidence.
