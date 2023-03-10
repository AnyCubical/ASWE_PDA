using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ASWE_PDA.Models.ApiServices;

public class ApiBase
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Constructors

    public ApiBase()
    {
        _httpClient = new HttpClient();
    }

    #endregion
    
    #region Public Methods

    protected async Task<string?> MakeHttpRequest(string url, string header = "", string value = "")
    {
        try
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                if(header != "" && value != "")
                    requestMessage.Headers.Add(header, value);
    
                var request = await _httpClient.SendAsync(requestMessage);

                using (var content = request.Content)
                {
                    return await content.ReadAsStringAsync();
                }
            }
        }
        catch
        {
            return null;
        }
    }
    
    #endregion
}