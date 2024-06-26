import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassSectionsComponent } from './class-sections.component';

describe('ClassSectionsComponent', () => {
  let component: ClassSectionsComponent;
  let fixture: ComponentFixture<ClassSectionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ClassSectionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassSectionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
