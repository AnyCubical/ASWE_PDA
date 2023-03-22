using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.GoogleCalendarApi;

public class GoogleCalendarApi : ApiBase
{
    #region Fields

    private static readonly GoogleCalendarApi? _instance = null;

    #endregion

    #region Constructors

    private GoogleCalendarApi()
    {
        
    }

    #endregion

    #region Public Methods

    public async Task<string?> GetMeetingsAsync()
    {
        try
        {
            UserCredential credential;

            await using (var stream = new System.IO.FileStream(@"E:\Uni\Sem6\ASWE\ASWE_PDA\ASWE_PDA\src\Models\ApiServices\GoogleCalendarApi\credentials.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    (await GoogleClientSecrets.FromStreamAsync(stream)).Secrets,
                    new[] { CalendarService.Scope.Calendar },
                    "user",
                    CancellationToken.None).Result;
            }
            
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Calendar API Example",
            });

            var now = DateTime.Now;
            var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endOfDay = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            
            var request = service.Events.List("primary");
            request.TimeMin = startOfDay;
            request.TimeMax = endOfDay;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            
            var events = await request.ExecuteAsync();
            Console.WriteLine("Upcoming events for today:");
            if (events.Items is not {Count: > 0}) 
                return null;

            var res = "";
            
            foreach (var eventItem in events.Items)
            {
                var start = eventItem.Start.DateTime?.ToString("hh:mm");
                var end = eventItem.End.DateTime?.ToString("hh:mm");
                res += $"Meeting Name: {eventItem.Summary} - from {start} to {end}\n";
            }

            return res;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    public static GoogleCalendarApi GetInstance()
    {
        return _instance ?? new GoogleCalendarApi();
    }

    #endregion
}