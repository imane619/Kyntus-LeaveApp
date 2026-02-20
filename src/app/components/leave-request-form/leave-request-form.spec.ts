import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveRequestForm } from './leave-request-form';

describe('LeaveRequestForm', () => {
  let component: LeaveRequestForm;
  let fixture: ComponentFixture<LeaveRequestForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeaveRequestForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LeaveRequestForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
