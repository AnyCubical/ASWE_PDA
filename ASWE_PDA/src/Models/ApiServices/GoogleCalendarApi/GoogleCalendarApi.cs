using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;

namespace ASWE_PDA.Models.ApiServices.GoogleCalendarApi
{
    public class GoogleCalendarApi : ApiBase
    {
        #region Fields

        private static readonly GoogleCalendarApi? Instance = null;

        #endregion

        #region Constructors

        private GoogleCalendarApi()
        {

        }

        #endregion

        #region Public Methods

        public async Task<IList<Event>?> GetMeetingsAsync()
        {
            try
            {
                UserCredential credential;

                await using (var stream = new System.IO.FileStream(@"C:\Users\nikla\Downloads\client_secret_342173377498-nb4hhe0ihqp7e000bm9stfjhmjhsvhlk.apps.googleusercontent.com.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                return events.Items;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string?> GetFormattedMeetingsAsync()
        {
            var events = await GetMeetingsAsync();

            if (events == null || events.Count == 0)
            {
                return null;
            }

            var res = "";

            foreach (var eventItem in events)
            {
                var start = eventItem.Start.DateTime?.ToString("hh:mm");
                var end = eventItem.End.DateTime?.ToString("hh:mm");
                res += $"Meeting Name: {eventItem.Summary} - from {start} to {end}\n";
            }

            return res;
        }

        public async Task<bool> IsMeetingInProgressAsync()
        {
            var events = await GetMeetingsAsync();
            if (events == null) return false;

            var now = DateTime.Now;

            foreach (var eventItem in events)
            {
                var start = eventItem.Start.DateTime;
                var end = eventItem.End.DateTime;

                if (start.HasValue && end.HasValue)
                {
                    if (now >= start.Value && now <= end.Value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static GoogleCalendarApi GetInstance()
        {
            return Instance ?? new GoogleCalendarApi();
        }

        #endregion
    }
}
