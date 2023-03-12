using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.ChuckNorrisApi;

public class CheckNorrisApi : ApiBase
{
    #region Fields

    private static readonly CheckNorrisApi? _instance = null;

    #endregion

    #region Constructors

    private CheckNorrisApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetJokeAsync()
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
    
    public static CheckNorrisApi GetInstance()
    {
        return _instance ?? new CheckNorrisApi();
    }
    
    #endregion
}