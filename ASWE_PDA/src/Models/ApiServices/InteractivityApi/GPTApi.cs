using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Linq;

namespace ASWE_PDA.Models.ApiServices.InteractivityApi;

public class GptApi : ApiBase
{

    #region Fields

    private static readonly GptApi? Instance = null;

    #endregion

    #region Constructors

    private GptApi()
    {
        
    }

    #endregion

    #region Public Methods
    
    public static GptApi GetInstance()
    {
        return Instance ?? new GptApi();
    }

    

    public async Task<string> GetGpt4ResponseAsync(string prompt, string conversationHistory)
    {
        string apiKey = ""; // Replace with your actual API key
        string apiUrl = "https://api.openai.com/v1/chat/completions"; // Replace with the appropriate API URL for GPT-4.0
        var model = "gpt-3.5-turbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json"); // Replace with your project name
            
            var messages = conversationHistory.Split("\n\n")
            .Select(line => new { content = line, role = line.StartsWith("User:") ? "user" : "assistant" })
            .ToArray();

            var requestBody = new
            {
                model,
                messages = messages.Concat(new[]
                {
                    new { content = prompt, role = "user" }
                }).ToArray()
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            JObject jsonResponse = JObject.Parse(responseString);
            return jsonResponse["choices"]![0]!["message"]!["content"]!.ToString();
        }
    }
    #endregion
}
