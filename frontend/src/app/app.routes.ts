import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'skill-matrix',
  },
  {
    path: 'skill-matrix',
    loadChildren: () =>
      import('./features/skill-matrix/skill-matrix.routes').then((m) => m.SKILL_MATRIX_ROUTES),
  },
  {
    path: '**',
    redirectTo: 'skill-matrix',
  },
];
