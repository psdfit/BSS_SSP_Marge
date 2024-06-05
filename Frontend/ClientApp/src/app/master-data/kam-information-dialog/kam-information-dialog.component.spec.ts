import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KAMInformationDialogComponent } from './kam-information-dialog.component';

describe('KAMInformationDialogComponent', () => {
  let component: KAMInformationDialogComponent;
  let fixture: ComponentFixture<KAMInformationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [KAMInformationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KAMInformationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
