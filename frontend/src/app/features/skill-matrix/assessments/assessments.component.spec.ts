import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { AssessmentsComponent } from './assessments.component';
import { SkillService } from '../data-access/skill.service';

describe('AssessmentsComponent', () => {
  let component: AssessmentsComponent;
  let fixture: ComponentFixture<AssessmentsComponent>;
  let skillService: jasmine.SpyObj<SkillService>;

  beforeEach(async () => {
    skillService = jasmine.createSpyObj('SkillService', ['getMyAssessments', 'submitAssessment']);
    skillService.getMyAssessments.and.returnValue(of({ items: [] }));
    skillService.submitAssessment.and.returnValue(of(undefined));

    await TestBed.configureTestingModule({
      imports: [AssessmentsComponent],
      providers: [{ provide: SkillService, useValue: skillService }],
    }).compileComponents();

    fixture = TestBed.createComponent(AssessmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load assessments on init', () => {
    expect(skillService.getMyAssessments).toHaveBeenCalled();
    expect(component.assessments).toEqual([]);
  });

  it('should display assessment type and status labels', () => {
    expect(component.getTypeLabel(0)).toBe('Self');
    expect(component.getTypeLabel(1)).toBe('Manager');
    expect(component.getStatusLabel(0)).toBe('Assigned');
    expect(component.getStatusLabel(1)).toBe('Submitted');
    expect(component.getStatusLabel(2)).toBe('Evaluated');
  });

  it('should call submitAssessment when submit is clicked', () => {
    component.assessments = [
      {
        id: 'a1',
        employeeSkillId: 'es1',
        assessmentType: 1,
        status: 0,
        dueDate: '2024-12-01',
        skillName: 'Angular',
      },
    ];
    component.submit('a1');
    expect(skillService.submitAssessment).toHaveBeenCalledWith('a1');
  });

  it('should reload assessments after submit success', () => {
    skillService.getMyAssessments.calls.reset();
    component.submit('a1');
    expect(skillService.getMyAssessments).toHaveBeenCalled();
  });
});
