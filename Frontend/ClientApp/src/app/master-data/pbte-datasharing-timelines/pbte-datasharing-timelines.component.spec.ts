import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PBTEDataSharingTimelinesComponent } from './pbte-datasharing-timelines.component';

describe('PBTEDataSharingTimelinesComponent', () => {
    let component: PBTEDataSharingTimelinesComponent;
    let fixture: ComponentFixture<PBTEDataSharingTimelinesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [PBTEDataSharingTimelinesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(PBTEDataSharingTimelinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
