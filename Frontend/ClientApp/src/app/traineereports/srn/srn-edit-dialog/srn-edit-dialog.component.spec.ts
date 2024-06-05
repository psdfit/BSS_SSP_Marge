import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SrnEditDialogComponent } from './srn-edit-dialog.component';

describe('SrnEditDialogComponent', () => {
  let component: SrnEditDialogComponent;
  let fixture: ComponentFixture<SrnEditDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SrnEditDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SrnEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
