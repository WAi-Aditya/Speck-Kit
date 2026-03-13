# Project Tech Stack

**Purpose**: Default technology choices for implementation plans. Used by `/speckit.plan` when filling Technical Context. Override via user input or feature-specific research when needed.

---

## Backend

| Item | Choice |
|------|--------|
| **Runtime / framework** | .NET Core (ASP.NET Core 8 recommended for new APIs) |
| **Persistence** | EF Core 8, SQL Server (or as specified per feature) |
| **Testing** | xUnit, NUnit, or MSTest; integration tests with WebApplicationFactory |

---

## Frontend

| Item | Choice |
|------|--------|
| **Framework** | Angular (17+ recommended) |
| **UI component library** | PrimeNG (for components, layout, and styling) |
| **CSS / styling** | PrimeNG themes + custom CSS where needed; keep styling consistent with PrimeNG design tokens |

---

## Theme & Design

| Item | Guidance |
|------|----------|
| **Visual style** | Professional and clean |
| **Principles** | Clear hierarchy, ample whitespace, readable typography, consistent spacing and colors. Avoid cluttered layouts and decorative excess. |
| **Accessibility** | Follow WCAG considerations; PrimeNG components used with semantic markup and ARIA where applicable. |

---

## Notes

- When generating plans or tasks, backend work MUST assume .NET Core; frontend MUST assume Angular with PrimeNG.
- All UI work MUST follow the **professional and clean** theme unless a feature spec explicitly requests a different style.
- Update this file when the project adopts a new default (e.g. .NET 9, Angular 18).
