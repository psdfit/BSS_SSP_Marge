import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EquipmentToolComponent } from './equipment-tool.component';

describe('EquipmentToolComponent', () => {
    let component: EquipmentToolComponent;
    let fixture: ComponentFixture<EquipmentToolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [EquipmentToolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(EquipmentToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
