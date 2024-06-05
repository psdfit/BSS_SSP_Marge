import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RTPDialogComponent } from './rtp.component';

describe('RTPDialogComponent', () => {
    let component: RTPDialogComponent;
    let fixture: ComponentFixture<RTPDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [RTPDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(RTPDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
