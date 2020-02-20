using System;
using ToDo.Core.Interfaces;
using ToDo.Core.Models;
using UserNotifications;

namespace ToDo.iOS.Implementations
{
    public class Reminder : IRemind
    {


        public void Remind(Note note)
        {
            var content = new UNMutableNotificationContent();
            content.Title = "You have to do an important thing";
            content.Subtitle = note.Header;
            content.Body = note.Content;
            content.Badge = 1;

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(2, false);

            var requestID = "sampleRequest";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => { });
        }
    }
}
