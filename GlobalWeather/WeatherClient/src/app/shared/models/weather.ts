import { CurrentConditions } from './current-conditions';
import { City } from './city';

export class Weather {
  public location: string;
  public weatherIconUrl: string;
  public weatherText: string;
  public temperatureValue: number;
  public temperatureUnit: string;
  public isDaytime: boolean;

  public constructor(currentConditions: CurrentConditions, city: City) {
    this.location = city.EnglishName;
    this.weatherText = currentConditions.WeatherText;
    this.isDaytime = currentConditions.IsDayTime;
    if (currentConditions.WeatherIcon)
      this.weatherIconUrl = `../assets/images/${currentConditions.WeatherIcon}.png`;
    this.temperatureValue = currentConditions.Temperature.Metric.Value;
    this.temperatureUnit = currentConditions.Temperature.Metric.Unit;
  }
}
