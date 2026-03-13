import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <div class="app-root">
      <header class="app-header">
        <h1>ITP — Skill Matrix</h1>
      </header>
      <main class="app-main">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styles: [
    `
      .app-root {
        min-height: 100vh;
        display: flex;
        flex-direction: column;
      }
      .app-header {
        padding: 0.75rem 1.5rem;
        background: #1e293b;
        color: #f8fafc;
      }
      .app-header h1 {
        margin: 0;
        font-size: 1.25rem;
        font-weight: 600;
      }
      .app-main {
        flex: 1;
        padding: 0;
      }
    `,
  ],
})
export class AppComponent {}
