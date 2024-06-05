import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPKAMHistoryDialogueComponent } from './tsp-kam-history-dialogue.component';

describe('TSPKAMHistoryDialogueComponent', () => {
    let component: TSPKAMHistoryDialogueComponent;
    let fixture: ComponentFixture<TSPKAMHistoryDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TSPKAMHistoryDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TSPKAMHistoryDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
