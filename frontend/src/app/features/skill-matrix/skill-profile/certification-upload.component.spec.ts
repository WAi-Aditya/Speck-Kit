import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { of, throwError } from 'rxjs';
import { CertificationUploadComponent } from './certification-upload.component';
import { SkillService } from '../data-access/skill.service';
import { MessageService } from 'primeng/api';

describe('CertificationUploadComponent', () => {
  let component: CertificationUploadComponent;
  let fixture: ComponentFixture<CertificationUploadComponent>;
  let skillService: jasmine.SpyObj<SkillService>;

  beforeEach(async () => {
    skillService = jasmine.createSpyObj('SkillService', ['getMyCertifications', 'uploadCertification']);
    skillService.getMyCertifications.and.returnValue(of({ items: [] }));
    skillService.uploadCertification.and.returnValue(
      of({
        id: 'c1',
        skillId: 's1',
        title: 'Test Cert',
        issuer: 'Issuer',
        issueDate: '2024-01-01',
      })
    );

    await TestBed.configureTestingModule({
      imports: [CertificationUploadComponent],
      providers: [
        { provide: SkillService, useValue: skillService },
        MessageService,
        provideHttpClient(),
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CertificationUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load certifications on init', () => {
    expect(skillService.getMyCertifications).toHaveBeenCalled();
    expect(component.certifications).toEqual([]);
  });

  it('should display certifications when loaded', async () => {
    skillService.getMyCertifications.and.returnValue(
      of({
        items: [
          { id: '1', skillId: 's1', title: 'AWS', issuer: 'Amazon', issueDate: '2024-01-01' },
        ],
      })
    );
    component.loadCertifications();
    fixture.detectChanges();
    expect(component.certifications.length).toBe(1);
    expect(component.certifications[0].title).toBe('AWS');
  });

  it('should call uploadCertification when file is uploaded', () => {
    const file = new File(['content'], 'cert.pdf', { type: 'application/pdf' });
    component.onUpload({ files: [file] });
    expect(skillService.uploadCertification).toHaveBeenCalledWith(
      file,
      jasmine.objectContaining({
        title: 'cert.pdf',
        issuer: '',
        skillId: '',
      })
    );
  });

  it('should set uploadError on upload failure', () => {
    skillService.uploadCertification.and.returnValue(
      throwError(() => ({ error: { detail: 'Invalid file type' } }))
    );
    const file = new File(['x'], 'x.pdf', { type: 'application/pdf' });
    component.onUpload({ files: [file] });
    expect(component.uploadError).toBe('Invalid file type');
  });

  it('should reload certifications after successful upload', () => {
    skillService.getMyCertifications.calls.reset();
    const file = new File(['x'], 'x.pdf', { type: 'application/pdf' });
    component.onUpload({ files: [file] });
    expect(skillService.getMyCertifications).toHaveBeenCalled();
  });
});
