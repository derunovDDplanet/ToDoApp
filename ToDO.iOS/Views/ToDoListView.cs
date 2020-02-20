using System;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Plugin.Messenger;
using ToDo.Core;
using ToDo.Core.Models;
using ToDo.Core.Services;
using ToDo.iOS.Sources;
using ToDo.iOS.Views.Cells;
using UIKit;
using UserNotifications;

namespace ToDo.iOS.Views
{
    public partial class ToDoListView : MvxViewController<ToDoListViewModel>
    {
        private Note Note;
        public ServiceBunch Service;
        private MvxSubscriptionToken _token;


        public ToDoListView() : base("ToDoListView", null)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ApplyBinding();
            TableView.TableHeaderView = SearchBar;
            _token = Service.Messenger.SubscribeOnMainThread<AlertDialogMessage>(ActionExecute);


        }

        private void ApplyBinding()
        {
            var set = this.CreateBindingSet<ToDoListView, ToDoListViewModel>();
            
            var source = new NotesTableViewSource(TableView);
            
            set.Bind(source).For(v => v.ItemsSource).To(vm => vm.SearchResult).OneWay();
            set.Bind(source).For(v => v.SelectionChangedCommand).To(vm => vm.NoteSelectedCommand);
            set.Bind(AddNoteButton).To(vm => vm.AddNoteCommand);
            set.Bind(source).For(s => s.ItemRemoveCommand).To(v => v.RemoveFromToDoListCommand);
            set.Bind(SearchBar).For(s => s.Text).To(v => v.SearchText);
            set.Bind(source).For(s => s.ActionSheetCommand).To(vm => vm.ActionSheetCommand);
            
            set.Apply();
            
            TableView.Source = source;
            TableView.ReloadData();

        }

        private void ActionExecute(AlertDialogMessage message)
        {
            var Note = message.Note;
            

            this.Note = Note;
            // Create a new Alert Controller
            UIAlertController actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);

            // Add Actions
            if (message.Actions != null)
            {
                foreach (var actionInfo in message.Actions)
                {
                    UIAlertActionStyle style = actionInfo.IsCancel ? UIAlertActionStyle.Cancel : UIAlertActionStyle.Default;
                    var alertAction = UIAlertAction.Create(actionInfo.Title, style,a => actionInfo.Action?.Invoke(Note));
                    actionSheetAlert.AddAction(alertAction);

                    //actionSheetAlert.AddAction(UIAlertAction.Create("Remind After 5 minutes", UIAlertActionStyle.Default, Remind));

                    //actionSheetAlert.AddAction(UIAlertAction.Create(CompletedLabel, UIAlertActionStyle.Default, (action) =>
                    //{
                    //    Note.IsDone = !Note.IsDone;
                    //}
                    //));

                    //actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                }
            }



            // Display the alert
            this.PresentViewController(actionSheetAlert, true, null);
        }

        //private void Remind(UIAlertAction action)
        //{
        //    var content = new UNMutableNotificationContent();
        //    content.Title = "You have to do an important thing";
        //    content.Subtitle = Note.Header;
        //    content.Body = Note.Content;
        //    content.Badge = 1;

        //    var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(2, false);

        //    var requestID = "sampleRequest";
        //    var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

        //    UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => { });
        //}
    }
}

