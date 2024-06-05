import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeRequestDialogComponent } from './change-request.component';

describe('ChangeRequestDialogComponent', () => {
    let component: ChangeRequestDialogComponent;
    let fixture: ComponentFixture<ChangeRequestDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [ChangeRequestDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(ChangeRequestDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
