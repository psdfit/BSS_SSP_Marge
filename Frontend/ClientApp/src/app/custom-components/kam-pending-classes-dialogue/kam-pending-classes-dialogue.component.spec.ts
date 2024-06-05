import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KAMPendingClassesDialogueComponent } from './kam-pending-classes-dialogue.component';

describe('KAMPendingClassesDialogueComponent', () => {
  let component: KAMPendingClassesDialogueComponent;
  let fixture: ComponentFixture<KAMPendingClassesDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [KAMPendingClassesDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KAMPendingClassesDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
