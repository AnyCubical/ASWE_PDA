using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.WeatherStationApi;

public class WeatherStationApi : ApiBase
{
    #region Fields

    private static readonly WeatherStationApi? Instance = null;

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
            
            var temperatureToken = json["10738"]?["days"];
            //var temperatureData = temperatureToken?.ToObject<String[]>();

            double minTemp = (double)temperatureToken[0]["temperatureMin"];
            double maxTemp = (double)temperatureToken[0]["temperatureMax"];
            double minTempTomorrow = (double)temperatureToken[1]["temperatureMin"];
            double maxTempTomorrow = (double)temperatureToken[1]["temperatureMax"];


            var res = $"Min Temp: {minTemp / 10}°C , Max Temp: {maxTemp / 10}°C, Tomorrows Temperature: Min Temp: {minTempTomorrow / 10}°C, Max Temp: {maxTempTomorrow/10}°C";

            return res;
        }
        catch (Exception e)
        {
            return null;
        }
        
    }
    
    public static WeatherStationApi GetInstance()
    {
        return Instance ?? new WeatherStationApi();
    }

    #endregion
}