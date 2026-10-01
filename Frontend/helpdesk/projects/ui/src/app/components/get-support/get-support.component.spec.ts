import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';

import { GetSupportComponent } from './get-support.component';

describe('GetSupportComponent', () => {
  let component: GetSupportComponent;
  let fixture: ComponentFixture<GetSupportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GetSupportComponent],
      imports: [ReactiveFormsModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show validation errors when required fields are missing', () => {
    component.submitRequest();

    expect(component.supportForm.invalid).toBeTrue();
    expect(component.attemptedSubmit).toBeTrue();
    expect(component.submitted).toBeFalse();
  });

  it('should confirm the form is ready when all fields are valid', () => {
    component.supportForm.setValue({
      fullName: 'Ada Lovelace',
      email: 'ada@example.com',
      category: 'technical',
      priority: 'normal',
      subject: 'Giriş sorunu',
      message: 'Hesabıma giriş yaparken hata alıyorum.'
    });

    component.submitRequest();

    expect(component.supportForm.valid).toBeTrue();
    expect(component.submitted).toBeTrue();
  });
});
