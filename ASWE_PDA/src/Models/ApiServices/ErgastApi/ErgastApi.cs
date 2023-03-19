using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ASWE_PDA.Models.ApiServices.ErgastApi;

public class ErgastApi : ApiBase
{
    #region Fields

    private static readonly ErgastApi? _instance = null;

    #endregion

    #region Constructors

    private ErgastApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetLatestF1ResultsAsync()
    {
        try
        {
            var response = await MakeHttpRequest(
                "https://ergast.com/api/f1/current/last/results"
            );

            if (string.IsNullOrEmpty(response))
                return null;

            var doc = XDocument.Parse(response);

            XNamespace ns = "http://ergast.com/mrd/1.5";
            var top3 = doc.Descendants(ns + "RaceTable")
                .Elements(ns + "Race")
                .Elements(ns + "ResultsList")
                .Elements(ns + "Result")
                .OrderBy(r => int.Parse(r.Attribute("position")?.Value!))
                .Take(3)
                .Select(r => r.Element(ns + "Driver")?.Element(ns + "GivenName")?.Value + " " +
                             r.Element(ns + "Driver")?.Element(ns + "FamilyName")?.Value);

            return top3.Aggregate("", (current, driver) => current + $"{driver}\n");
        }
        catch
        {
            return null;
        }
        
    }
    
    public static ErgastApi GetInstance()
    {
        return _instance ?? new ErgastApi();
    }

    #endregion
}