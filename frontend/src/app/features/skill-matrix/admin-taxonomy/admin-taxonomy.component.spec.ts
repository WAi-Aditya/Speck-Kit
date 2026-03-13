import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { AdminTaxonomyComponent } from './admin-taxonomy.component';
import { SkillService } from '../data-access/skill.service';

describe('AdminTaxonomyComponent', () => {
  let component: AdminTaxonomyComponent;
  let fixture: ComponentFixture<AdminTaxonomyComponent>;
  let skillService: jasmine.SpyObj<SkillService>;

  beforeEach(async () => {
    skillService = jasmine.createSpyObj('SkillService', [
      'getCategories',
      'getSubCategories',
      'getSkills',
    ]);
    skillService.getCategories.and.returnValue(of({ items: [{ id: 'c1', name: 'Development', isActive: true, sortOrder: 1 }] }));
    skillService.getSubCategories.and.returnValue(of({ items: [{ id: 's1', skillCategoryId: 'c1', name: 'Frontend', isActive: true, sortOrder: 1 }] }));
    skillService.getSkills.and.returnValue(of({ items: [{ id: 'k1', skillSubCategoryId: 's1', name: 'React', isActive: true, sortOrder: 1 }] }));
    await TestBed.configureTestingModule({
      imports: [AdminTaxonomyComponent],
      providers: [{ provide: SkillService, useValue: skillService }],
    }).compileComponents();
    fixture = TestBed.createComponent(AdminTaxonomyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load taxonomy on init', () => {
    expect(skillService.getCategories).toHaveBeenCalled();
    expect(skillService.getSubCategories).toHaveBeenCalled();
    expect(skillService.getSkills).toHaveBeenCalled();
    expect(component.categories.length).toBe(1);
    expect(component.categories[0].name).toBe('Development');
    expect(component.skills[0].name).toBe('React');
  });
});
