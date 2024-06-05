import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPPendingClassesDialogueComponent } from './tsp-pending-classes-dialogue.component';

describe('TSPPendingClassesDialogueComponent', () => {
    let component: TSPPendingClassesDialogueComponent;
    let fixture: ComponentFixture<TSPPendingClassesDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TSPPendingClassesDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TSPPendingClassesDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
