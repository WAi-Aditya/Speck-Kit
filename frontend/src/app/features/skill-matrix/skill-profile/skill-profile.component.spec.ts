import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { SkillProfileComponent } from './skill-profile.component';
import { SkillService } from '../data-access/skill.service';

describe('SkillProfileComponent', () => {
  let component: SkillProfileComponent;
  let fixture: ComponentFixture<SkillProfileComponent>;
  let skillService: jasmine.SpyObj<SkillService>;

  beforeEach(async () => {
    skillService = jasmine.createSpyObj('SkillService', ['getMyProfile', 'updateProfileSkill']);
    skillService.getMyProfile.and.returnValue(
      of({
        employeeId: 'e1',
        skills: [
          {
            employeeSkillId: 'es1',
            skillId: 's1',
            skillName: 'React',
            categoryName: 'Development',
            subCategoryName: 'Frontend',
            selfAssessedLevel: 2,
            validationStatus: 0,
          },
        ],
      })
    );
    await TestBed.configureTestingModule({
      imports: [SkillProfileComponent],
      providers: [{ provide: SkillService, useValue: skillService }],
    }).compileComponents();
    fixture = TestBed.createComponent(SkillProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load profile on init', () => {
    expect(skillService.getMyProfile).toHaveBeenCalled();
    expect(component.profile?.employeeId).toBe('e1');
    expect(component.profile?.skills.length).toBe(1);
    expect(component.profile?.skills[0].skillName).toBe('React');
  });

  it('should show status label', () => {
    expect(component.getStatusLabel(0)).toBe('Pending');
    expect(component.getStatusLabel(1)).toBe('Approved');
    expect(component.getStatusLabel(2)).toBe('Rejected');
  });
});
