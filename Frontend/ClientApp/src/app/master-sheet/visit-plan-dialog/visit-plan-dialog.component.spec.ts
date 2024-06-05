import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitPlanDialogComponent } from './visit-plan-dialog.component';

describe('VisitPlanDialogComponent', () => {
  let component: VisitPlanDialogComponent;
  let fixture: ComponentFixture<VisitPlanDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VisitPlanDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VisitPlanDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
