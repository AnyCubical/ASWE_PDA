using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.CoinPaprikaApi;

public class CoinPaprikaApi : ApiBase
{
    #region Fields

    private static readonly CoinPaprikaApi? Instance = null;

    #endregion

    #region Constructors

    private CoinPaprikaApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<Tuple<double, double>?> GetBitcoinEthereumPriceDollarAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://api.coinpaprika.com/v1/tickers"
            );

            if (response == null)
                return null;
            
            var bJsonArray = JArray.Parse(response);
            var bJObject = (JObject)bJsonArray?[0]?["quotes"]?["USD"]!;
        
            var bitcoinPrice = (double)(bJObject["price"] ?? 0);
            
            var eJsonArray = JArray.Parse(response);
            var eJObject = (JObject)eJsonArray?[1]?["quotes"]?["USD"]!;
        
            var ethereumPrice = (double)(eJObject["price"] ?? 0);
            
            return new Tuple<double, double>(bitcoinPrice, ethereumPrice);
        }
        catch
        {
            return null;
        }
    }

    public static CoinPaprikaApi GetInstance()
    {
        return Instance ?? new CoinPaprikaApi();
    }

    #endregion
}