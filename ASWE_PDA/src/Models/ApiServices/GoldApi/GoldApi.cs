using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ASWE_PDA.Models.ApiServices.GoldApi;

public class GoldApi : ApiBase
{
    #region Fields

    private static readonly GoldApi? _instance = null;
    private const string Token = "x-access-token";
    private const string TokenValue = "goldapi-ddpfrlf28vuoo-io";

    #endregion

    #region Constructors

    private GoldApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<Tuple<double, double>?> GetGoldSliverPriceDollarAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://www.goldapi.io/api/XAU/USD",
                Token,
                TokenValue
            );

            if (response == null)
                return null;
        
            var json = JsonNode.Parse(response);
            var gold = (double)(json?["price"] ?? 0);
        
            response = await MakeHttpRequest(
                "https://www.goldapi.io/api/XAG/USD",
                Token,
                TokenValue
            );

            if (response == null)
                return null;
        
            json = JsonNode.Parse(response);
            var silver = (double)(json?["price"] ?? 0);
        
            return new Tuple<double, double>(gold, silver);
        }
        catch
        {
            return new Tuple<double, double>(0, 0);
        }
    }
    
    public static GoldApi GetInstance()
    {
        return _instance ?? new GoldApi();
    }

    #endregion
}