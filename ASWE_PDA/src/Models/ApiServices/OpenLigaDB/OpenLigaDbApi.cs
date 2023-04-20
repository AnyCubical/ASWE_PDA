using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.OpenLigaDB;

public class OpenLigaDbApi : ApiBase
{
    #region Fields

    private static readonly OpenLigaDbApi? Instance = null;

    #endregion

    #region Constructors

    private OpenLigaDbApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetLeadingTeamsAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://api.openligadb.de/getbltable/bl1/2022"
            );

            if (response == null)
                return null;
        
            var json = JArray.Parse(response)!;

            var topThreeTeams = json.OrderByDescending(t => (int) t["points"]!).Take(3);

            var res = "";
        
            foreach (var team in topThreeTeams)
                res += $"Team: {(string)team["teamName"]!}, Punkte: {(int)team["points"]!} \n";

            return res;
        }
        catch
        {
            return null;
        }
        
    }
    
    public static OpenLigaDbApi GetInstance()
    {
        return Instance ?? new OpenLigaDbApi();
    }

    #endregion
}