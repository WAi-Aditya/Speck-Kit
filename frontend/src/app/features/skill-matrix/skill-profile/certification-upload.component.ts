import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SkillService, CertificationDto } from '../data-access/skill.service';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-certification-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, FileUploadModule, ToastModule],
  providers: [MessageService],
  template: `
    <p-toast />
    <div class="certification-upload">
      <h3>My Certifications</h3>
      <p *ngIf="loading">Loading...</p>
      <p-fileUpload
        *ngIf="!loading"
        name="certFile"
        [multiple]="false"
        accept=".pdf,.jpg,.jpeg,.png"
        (uploadHandler)="onUpload($event)"
        chooseLabel="Choose file"
        uploadLabel="Upload"
      />
      <p *ngIf="uploadError" class="upload-error">{{ uploadError }}</p>
      <ul *ngIf="certifications.length">
        <li *ngFor="let c of certifications">{{ c.title }} ({{ c.issuer }})</li>
      </ul>
    </div>
  `,
  styles: [
    `
      .certification-upload { margin-top: 1rem; }
      .upload-error { color: var(--p-danger-color); }
    `,
  ],
})
export class CertificationUploadComponent implements OnInit {
  certifications: CertificationDto[] = [];
  loading = true;
  uploadError: string | null = null;

  constructor(
    private readonly skillService: SkillService,
    private readonly messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadCertifications();
  }

  loadCertifications(): void {
    this.loading = true;
    this.uploadError = null;
    this.skillService.getMyCertifications().subscribe({
      next: (r) => {
        this.certifications = r?.items ?? [];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
  }

  onUpload(event: { files: File[] }): void {
    const file = event?.files?.[0];
    if (!file) return;
    this.uploadError = null;
    this.skillService
      .uploadCertification(file, {
        skillId: '',
        title: file.name,
        issuer: '',
        issueDate: new Date().toISOString().slice(0, 10),
      })
      .subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Uploaded', detail: 'Certification uploaded.' });
          this.loadCertifications();
        },
        error: (err) => {
          this.uploadError = err?.error?.detail ?? 'Upload failed';
        },
      });
  }
}
