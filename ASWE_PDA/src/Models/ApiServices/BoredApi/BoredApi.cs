using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.BoredApi;

public class BoredApi : ApiBase
{
    #region Fields

    private static readonly BoredApi? _instance = null;

    #endregion

    #region Constructors

    private BoredApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetActivityAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://www.boredapi.com/api/activity"
            );

            if (response == null)
                return null;
        
            var json = JObject.Parse(response)!;
            var res = (string)json["activity"]!;

            return res;
        }
        catch
        {
            return null;
        }
        
    }
    
    public static BoredApi GetInstance()
    {
        return _instance ?? new BoredApi();
    }

    #endregion
}