import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Constants } from '../../../app/app.constants';
import { Country } from '../../shared/models/country';
import { City } from '../../shared/models/city';
import { catchError, map, tap } from 'rxjs/operators';
import { ErrorHandleService } from '../../shared/services/error-handle.service';


@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(
    private http: HttpClient,
    private errorHandleService: ErrorHandleService) { }

  getCountries(): Observable<Country[]> {
    const uri = decodeURIComponent(
      `${Constants.locationAPIUrl}/countries?apikey=${Constants.apiKey}`
    );
    return this.http.get<Country[]>(uri)
      .pipe(
        tap(_ => console.log('fetched countries')),
        catchError(this.errorHandleService.handleError('getCountries', []))
      );
  }

  getCities(searchText: string, countryCode: string): Observable<City[]> {
    const uri = countryCode
      ? decodeURIComponent(
        `${Constants.locationAPIUrl}/cities/${countryCode}/search?apikey=${Constants.apiKey}&q=${searchText}`)
      : decodeURIComponent(
        `${Constants.locationAPIUrl}/cities/search?apikey=${Constants.apiKey}&q=${searchText}`);
    return this.http.get<City[]>(uri)
      .pipe(
        map(res => (res as City[]).map(o => {
          return {
            Key: o.Key,
            EnglishName: o.EnglishName,
            Type: o.Type,
            Country: {
              ID: o.Country.ID,
              EnglishName: o.Country.EnglishName
            }
          }

        })),
        tap(_ => console.log('fetched cities')),
        catchError(this.errorHandleService.handleError('getCities', []))
      );
  }

}
