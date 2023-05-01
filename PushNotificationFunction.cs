using Carllet_Notification_Job.Data;
using Expo.Server.Client;
using Expo.Server.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NotificationService
{
    public class PushNotificationFunction
    {
        private readonly ILogger _logger;

        public PushNotificationFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PushNotificationFunction>();
        }

        [Function("PushNotificationFunction")]
        public async Task RunAsync([TimerTrigger("*/20 * * * * *")] MyInfo myTimer, ILogger myLog)
        {
            _logger.LogInformation($"Notificações enviadas - Horário atual: {DateTime.Now}");
            var _db = new DbService();

            List<User> userNotif = await _db.GetUsers();

            var pushApi = new PushApiClient();

            userNotif.ForEach(async user =>
            {
                var pushTicketReq = new PushTicketRequest()
                {
                    PushTo = $"ExponentPushToken[{user.DeviceId}]",
                    PushTitle = "Manutenção Pendente",
                    PushBody = $"{user.Name}: Você tem manutenção pendente"
                };

                var result = await pushApi.PushSendAsync(pushTicketReq);
            });

        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
