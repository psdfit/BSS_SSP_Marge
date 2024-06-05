import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SapBranchesComponent } from './sap-branches.component';

describe('SapBranchesComponent', () => {
  let component: SapBranchesComponent;
  let fixture: ComponentFixture<SapBranchesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SapBranchesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SapBranchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
