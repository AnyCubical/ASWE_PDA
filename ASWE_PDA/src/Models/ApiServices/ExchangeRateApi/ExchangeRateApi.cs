using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.ExchangeRateApi;

public class ExchangeRateApi : ApiBase
{
    #region Fields

    private static readonly ExchangeRateApi? _instance = null;

    #endregion

    #region Constructors

    private ExchangeRateApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<double?> GetUSDtoEURAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://v6.exchangerate-api.com/v6/f289754a342d0ec30998b32e/latest/USD"
            );

            if (response == null)
                return null;
            
            var jObject = JObject.Parse(response);
            var conversion = (double)jObject["conversion_rates"]?["EUR"]!;

            return conversion;
        }
        catch
        {
            return null;
        }
    }
    
    public static ExchangeRateApi GetInstance()
    {
        return _instance ?? new ExchangeRateApi();
    }

    #endregion
}