import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PotentialTraineesListComponent } from './potential-trainees-list.component';

describe('PotentialTraineesListComponent', () => {
  let component: PotentialTraineesListComponent;
  let fixture: ComponentFixture<PotentialTraineesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PotentialTraineesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PotentialTraineesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
