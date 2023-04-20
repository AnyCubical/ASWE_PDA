using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.ChuckNorrisApi;

public class ChuckNorrisApi : ApiBase
{
    #region Fields

    private static readonly ChuckNorrisApi? Instance = null;

    #endregion

    #region Constructors

    private ChuckNorrisApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetJokeAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://api.chucknorris.io/jokes/random"
            );

            if (response == null)
                return null;
        
            var json = JObject.Parse(response)!;
            var res = (string)json["value"]!;

            return res;
        }
        catch
        {
            return null;
        }
    }
    
    public static ChuckNorrisApi GetInstance()
    {
        return Instance ?? new ChuckNorrisApi();
    }
    
    #endregion
}