import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PSPAssignedTraineesListComponent } from './psp-assigned-trainees-list.component';

describe('PSPAssignedTraineesListComponent', () => {
  let component: PSPAssignedTraineesListComponent;
  let fixture: ComponentFixture<PSPAssignedTraineesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PSPAssignedTraineesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PSPAssignedTraineesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
