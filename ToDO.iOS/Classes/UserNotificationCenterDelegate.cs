using System;
using UserNotifications;

namespace ToDo.iOS.Classes
{
	public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
	{
		
		public UserNotificationCenterDelegate()
		{
		}
	

		
		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			

			// Tell system to display the notification anyway or use
			// `None` to say we have handled the display locally.
			completionHandler(UNNotificationPresentationOptions.Alert);
		}

		public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
		
			
			// Inform caller it has been handled
			completionHandler();
		}
		
	}
}
