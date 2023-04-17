using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text;
using System;

namespace ASWE_PDA.Models.ApiServices.InteractivityApi;

public class GPTApi : ApiBase
{

    #region Fields

    private static readonly GPTApi? _instance = null;

    #endregion

    #region Constructors

    private GPTApi()
    {
        
    }

    #endregion

    #region Public Methods
    
    public static GPTApi GetInstance()
    {
        return _instance ?? new GPTApi();
    }

    

    public async Task<string> GetGPT4ResponseAsync(string prompt)
    {
        string apiKey = ""; // Replace with your actual API key
        string apiUrl = "https://api.openai.com/v1/chat/completions"; // Replace with the appropriate API URL for GPT-4.0
        //var prompt = "Hello, my name is";
        var model = "gpt-3.5-turbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json"); // Replace with your project name

            var requestBody = new
            {
                model,
                messages = new[]
                {
                    new
                    {
                        content = prompt,
                        role = "user"
                    }
                }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            JObject jsonResponse = JObject.Parse(responseString);
            return jsonResponse["choices"][0]["message"]["content"].ToString();
        }
    }
    #endregion
}
