# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]
**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

[Extract from feature spec: primary requirement + technical approach from research]

## Technical Context

<!--
  Fill from .specify/memory/tech-stack.md when present. Default for this project:
  Backend: .NET Core (ASP.NET Core 8). Frontend: Angular 17+ with PrimeNG. Theme: professional and clean.
-->

**Backend**: [e.g., .NET Core / ASP.NET Core 8, or NEEDS CLARIFICATION]  
**Frontend / UI**: [e.g., Angular 17+ with PrimeNG for components and styling, or NEEDS CLARIFICATION]  
**Theme / Design**: [e.g., Professional and clean — clear hierarchy, whitespace, readable typography; or NEEDS CLARIFICATION]  
**Language/Version**: [backend/frontend versions if not covered above]  
**Primary Dependencies**: [e.g., EF Core 8, PrimeNG 17, or NEEDS CLARIFICATION]  
**Storage**: [if applicable, e.g., SQL Server, or N/A]  
**Testing**: [e.g., xUnit/NUnit, Jasmine/Karma, or NEEDS CLARIFICATION]  
**Target Platform**: [e.g., Linux/Windows server, modern browsers, or NEEDS CLARIFICATION]  
**Project Type**: [e.g., web application (backend + frontend), or NEEDS CLARIFICATION]  
**Performance Goals**: [domain-specific]  
**Constraints**: [domain-specific]  
**Scale/Scope**: [domain-specific]

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
