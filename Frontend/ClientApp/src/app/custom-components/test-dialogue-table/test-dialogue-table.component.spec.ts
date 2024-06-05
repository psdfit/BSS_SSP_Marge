import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestDialogueTableComponent } from './test-dialogue-table.component';

describe('TestDialogueTableComponent', () => {
  let component: TestDialogueTableComponent;
  let fixture: ComponentFixture<TestDialogueTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestDialogueTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestDialogueTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
