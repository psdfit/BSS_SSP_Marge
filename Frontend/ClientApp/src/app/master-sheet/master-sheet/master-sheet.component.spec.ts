import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterSheetComponent } from './master-sheet.component';

describe('MasterSheetComponent', () => {
  let component: MasterSheetComponent;
  let fixture: ComponentFixture<MasterSheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [MasterSheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterSheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
