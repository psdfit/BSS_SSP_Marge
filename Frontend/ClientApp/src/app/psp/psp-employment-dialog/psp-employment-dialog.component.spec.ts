import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PSPEmploymentDialogComponent } from './psp-employment-dialog.component';

describe('PSPEmploymentDialogComponent', () => {
  let component: PSPEmploymentDialogComponent;
  let fixture: ComponentFixture<PSPEmploymentDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PSPEmploymentDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PSPEmploymentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
