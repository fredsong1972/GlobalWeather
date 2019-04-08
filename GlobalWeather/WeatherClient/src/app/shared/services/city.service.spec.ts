import { async, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController, TestRequest } from '@angular/common/http/testing';
import { Constants } from '../../../app/app.constants';
import { CityService } from './city.service';
import { ErrorHandleService } from './error-handle.service';
import { CityMetaData } from '../models/city-meta-data';
import { City } from '../models/city';

describe('CityService', () => {
  let service: CityService;
  let httpTestingController: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CityService, ErrorHandleService]
    });
    service = TestBed.get(CityService);
    httpTestingController = TestBed.get(HttpTestingController);
  }));

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should create', () => {
    expect(service).toBeTruthy();
  });

  it('should get last accessed city', () => {
    const result = { id: '26216', name: 'Melbourne', countryId: 'AU' } as CityMetaData;

    service.getLastAccessedCity()
      .subscribe(
        (data: City) => expect(data.Key).toEqual('26216'),
        (err) => expect(err).toBeNull()
      );
    const uri = decodeURIComponent(`${Constants.cityAPIUrl}`);
    const req: TestRequest = httpTestingController.expectOne(req => req.url.includes(uri));

    expect(req.request.method).toEqual('GET');

    req.flush(result);
  });
});
