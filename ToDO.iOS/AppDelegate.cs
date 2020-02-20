using Foundation;
using UserNotifications;
using MvvmCross.Platforms.Ios.Core;

using ToDo.Core;
using UIKit;
using ToDo.iOS.Classes;
using MvvmCross;
using ToDo.iOS.Implementations;
using ToDo.Core.Interfaces;

namespace ToDo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate<MvxIosSetup<App>, App>
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);

            // Request notification permissions from the user
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) => {
                // Handle approval
            });

            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

            Mvx.IoCProvider.RegisterType<IRemind, Reminder>();

            return result;
        }

        
    }
}

