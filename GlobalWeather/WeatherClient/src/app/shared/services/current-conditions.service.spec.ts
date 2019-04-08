import { async, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController, TestRequest } from '@angular/common/http/testing';
import { Constants } from '../../../app/app.constants';
import { CurrentConditionsService } from './current-conditions.service';
import { ErrorHandleService } from './error-handle.service';
import { CurrentConditions } from '../models/current-conditions';

describe(' CurrentConditionsService', () => {
  let service: CurrentConditionsService;
  let httpTestingController: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CurrentConditionsService, ErrorHandleService]
    });
    service = TestBed.get(CurrentConditionsService);
    httpTestingController = TestBed.get(HttpTestingController);
  }));

  afterEach(() => {
    httpTestingController.verify();
  });


  it('should create',
    () => {
      expect(service).toBeTruthy();
    });

  it('should get current conditions',
    () => {
      const result = [
        {
          LocalObservationDateTime: '',
          WeatherText: 'Sunny',
          WeatherIcon: 1,
          IsDayTime: true,
          Temperature: {
            Imperial: null,
            Metric: {
              Unit: 'C',
              UnitType: 1,
              Value: 36
            }
          }

        }
      ] as CurrentConditions[];


      service.getCurrentConditions('26216')
        .subscribe(
          (data: CurrentConditions[]) => expect(data.length === 1 && data[0].WeatherText === 'Sunny').toBeTruthy(),
          (err: CurrentConditions[]) => expect(err.length).toEqual(0)
        );
      const uri = decodeURIComponent(`${Constants.currentConditionsAPIUrl}/26216?apikey=${Constants.apiKey}`);
      const req: TestRequest = httpTestingController.expectOne(req => req.url.includes(uri));

      expect(req.request.method).toEqual('GET');

      req.flush(result);
    });
});
