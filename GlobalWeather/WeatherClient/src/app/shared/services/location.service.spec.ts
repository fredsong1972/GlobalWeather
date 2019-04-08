import { async, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController, TestRequest } from '@angular/common/http/testing';
import { Constants } from '../../../app/app.constants';
import { LocationService } from './location.service';
import { ErrorHandleService } from './error-handle.service';
import { Country } from '../../shared/models/country';
import { City } from '../../shared/models/city';

describe('LocationService', () => {
  let service: LocationService;
  let httpTestingController: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [LocationService, ErrorHandleService]
    });
    service = TestBed.get(LocationService);
    httpTestingController = TestBed.get(HttpTestingController);
  }));

  afterEach(() => {
    httpTestingController.verify();
  });


  it('should create', () => {
    expect(service).toBeTruthy();
  });

  it('should get location', () => {
    const result = [{
      Key: '26216', EnglishName: 'Melbourne', Type: 'City', Country: {
        ID: 'AU',
        EnglishName: 'Australia'
      }
    }] as City[];

    service.getCities('melbourne', 'AU')
      .subscribe(
        (data: City[]) => expect(data.length === 1 && data[0].Key === '26216').toBeTruthy(),
        (err: City[]) => expect(err.length).toEqual(0)
      );
    const uri = decodeURIComponent(
      `${Constants.locationAPIUrl}/cities/AU/search?apikey=${Constants.apiKey}&q=melbourne`);
    const req: TestRequest = httpTestingController.expectOne(req => req.url.includes(uri));

    expect(req.request.method).toEqual('GET');

    req.flush(result);
  });

  it('should get countries', () => {
    const result = [{
      ID: 'AU', EnglishName: 'Australia'
    }] as Country[];

    service.getCountries()
      .subscribe(
        (data: Country[]) => expect(data.length === 1 && data[0].ID === 'AU').toBeTruthy(),
        (err: Country[]) => expect(err.length).toEqual(0)
      );
    const uri = decodeURIComponent(`${Constants.locationAPIUrl}/countries?apikey=${Constants.apiKey}`);
    const req: TestRequest = httpTestingController.expectOne(req => req.url.includes(uri));

    expect(req.request.method).toEqual('GET');

    req.flush(result);
  });
});
