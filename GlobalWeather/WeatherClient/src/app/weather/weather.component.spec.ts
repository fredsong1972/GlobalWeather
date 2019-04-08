import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule, FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { WeatherComponent } from './weather.component';
import { LocationService } from '../shared/services/location.service';
import { CurrentConditionsService } from '../shared/services/current-conditions.service';
import { CityService } from '../shared/services/city.service';
import { ErrorHandleService } from '../shared/services/error-handle.service';

describe('WeatherComponent', () => {
  let component: WeatherComponent;
  let fixture: ComponentFixture<WeatherComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [WeatherComponent],
      imports: [ReactiveFormsModule, NgbModule, RouterTestingModule, HttpClientTestingModule],
      providers: [LocationService, CurrentConditionsService, CityService, ErrorHandleService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(WeatherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get invalid form when location field is empty ',
    () => {
      component.ngOnInit();
      expect(component.weatherForm.valid).toEqual(false);
    });

  it('should get valid form when location field has value ',
    () => {
      component.ngOnInit();
      component.cityControl.patchValue("something");
      expect(component.weatherForm.valid).toEqual(true);
    });
});
