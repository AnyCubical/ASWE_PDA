using ASWE_PDA.Models.ApiServices.BoredApi;
using ASWE_PDA.Models.ApiServices.CatFactApi;
using ASWE_PDA.Models.ApiServices.ChuckNorrisApi;
using ASWE_PDA.Models.ApiServices.CoinPaprikaApi;
using ASWE_PDA.Models.ApiServices.ErgastApi;
using ASWE_PDA.Models.ApiServices.ExchangeRateApi;
using ASWE_PDA.Models.ApiServices.GoldApi;
using ASWE_PDA.Models.ApiServices.OpenLigaDB;
using ASWE_PDA.Models.ApiServices.WeatherStationApi;
using ASWE_PDA.Models.ApiServices.WitzApi;

namespace ASWE_PDA_Test;

public class ApiTests
{
    [Fact]
    public async void CatFactApiTest()
    {
        var api = CatFactApi.GetInstance();
        var result = await api.GetCatFactAsync();

        Assert.NotNull(result); 
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void ChuckNorrisApiTest()
    {
        var api = ChuckNorrisApi.GetInstance();
        var result = await api.GetJokeAsync();

        Assert.NotNull(result); 
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void CoinPaprikaApiTest()
    {
        var api = CoinPaprikaApi.GetInstance();
        var result = await api.GetBitcoinEthereumPriceDollarAsync();

        Assert.NotNull(result); 
        Assert.IsType<Tuple<double, double>>(result);
    }
    
    [Fact]
    public async void ErgastApiTest()
    {
        var api = ErgastApi.GetInstance();
        var result = await api.GetLatestF1ResultsAsync();

        Assert.NotNull(result); 
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void ExchangeRateApiTest()
    {
        var api = ExchangeRateApi.GetInstance();
        var result = await api.GetUSDtoEURAsync();

        Assert.NotNull(result); 
        Assert.IsType<double>(result);
    }
    
    [Fact]
    public async void GoldApiTest()
    {
        var api = GoldApi.GetInstance();
        var result = await api.GetGoldSliverPriceDollarAsync();

        Assert.NotNull(result); 
        Assert.IsType<Tuple<double, double>>(result);
    }
    
    [Fact]
    public async void OpenLigaDBApiTest()
    {
        var api = OpenLigaDbApi.GetInstance();
        var result = await api.GetLeadingTeamsAsync();

        Assert.NotNull(result); 
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void WitzApiTest()
    {
        var api = WitzApi.GetInstance();
        var result = await api.GetJokeAsync();

        Assert.NotNull(result); 
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void BoredApiTest()
    {
        var api = BoredApi.GetInstance();
        var result = await api.GetActivityAsync();

        Assert.NotNull(result); 
        Assert.IsType<string>(result);
    }
    
    public async void WeatherApiTest()
    {
        var api = WeatherStationApi.GetInstance();
        var result = await api.GetWeatherAsync();

        Assert.NotNull(result); 
        Assert.IsType<string>(result);
    }
}