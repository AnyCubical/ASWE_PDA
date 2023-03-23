using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.WeatherStationApi;

public class WeatherStationApi : ApiBase
{
    #region Fields

    private static readonly WeatherStationApi? _instance = null;

    #endregion

    #region Constructors

    private WeatherStationApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetWeatherAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://dwd.api.proxy.bund.dev/v30/stationOverviewExtended?stationIds=10738"
            );

            if (response == null)
                return null;
            
            var json = JObject.Parse(response)!;
            
            var temperatureToken = json["10738"]?["forecast1"]?["temperatureStd"];
            var temperatureData = temperatureToken?.ToObject<int[]>();
            
            var precipitationToken = json["10738"]?["forecast1"]?["precipitationTotal"];
            var precipitationData = precipitationToken?.ToObject<int[]>();

            var minTemp = double.MaxValue;
            var maxTemp = double.MinValue;
            
            foreach (var temp in temperatureData!)
            {
                if(temp is < -200 or > 150)
                    continue;
                    
                minTemp = temp < minTemp ? temp : minTemp;
                maxTemp = temp > maxTemp ? temp : maxTemp;
            }
            
            var minPerc = double.MaxValue;
            var maxPerc = double.MinValue;
            
            foreach (var perc in precipitationData!)
            {
                minPerc = perc < minPerc ? perc : minPerc;
                maxPerc = perc > maxPerc ? perc : maxPerc;
            }

            var res = $"Min Temp: {minTemp / 10}°F , Max Temp: {maxTemp / 10}°F, Min Precipitation {minPerc / 32767}%, Max Precipitation {maxPerc/32767}%";

            return res;
        }
        catch (Exception e)
        {
            return null;
        }
        
    }
    
    public static WeatherStationApi GetInstance()
    {
        return _instance ?? new WeatherStationApi();
    }

    #endregion
}