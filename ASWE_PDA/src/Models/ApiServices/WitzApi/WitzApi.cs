using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.WitzApi;

public class WitzApi : ApiBase
{
    #region Fields

    private static readonly WitzApi? _instance = null;

    #endregion

    #region Constructors

    private WitzApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetJokeAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://witzapi.de/api/joke/?limit=1&language=de"
            );

            if (response == null)
                return null;
        
            var json = JArray.Parse(response)!;
            var res = (string)json[0]["text"]!;

            return res;
        }
        catch
        {
            return null;
        }
    }
    
    public static WitzApi GetInstance()
    {
        return _instance ?? new WitzApi();
    }

    #endregion
}