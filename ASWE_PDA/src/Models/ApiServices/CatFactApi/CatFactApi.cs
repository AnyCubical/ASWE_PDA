using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.CatFactApi;

public class CatFactApi : ApiBase
{
    #region Fields

    private static readonly CatFactApi? _instance = null;

    #endregion

    #region Constructors

    private CatFactApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetCatFactAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://catfact.ninja/fact"
            );

            if (response == null)
                return null;
        
            var json = JObject.Parse(response)!;
            var res = (string)json["fact"]!;

            return res;
        }
        catch
        {
            return null;
        }
        
    }
    
    public static CatFactApi GetInstance()
    {
        return _instance ?? new CatFactApi();
    }

    #endregion
}