using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Views;
using ToDo.Core;
using UIKit;
using UserNotifications;


namespace ToDo.iOS.Views
{
    public partial class AddNoteView : MvxViewController<AddNoteViewModel>
    {
        private bool _isDone;
        public bool IsDone { get => _isDone; set=> _isDone = value;  }
        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; } }

        public IMvxCommand CompletedCommand { get; set; }

        public AddNoteView() : base("AddNoteView", null)
        {

        }

        public override void ViewDidLoad()
        {
            
            base.ViewDidLoad();
            ApplyBinding();
            CompleteUI();

            ActionsButton.TouchUpInside+=((sender, e) =>
            {

                // Create a new Alert Controller
                UIAlertController actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);

                // Add Actions
                if(IsEditing) actionSheetAlert.AddAction(UIAlertAction.Create("Remind After 5 minutes", UIAlertActionStyle.Default, Do));

                actionSheetAlert.AddAction(UIAlertAction.Create("Completed", UIAlertActionStyle.Default, (action)=>
                {
                    IsDone = !IsDone;
                    CompletedCommand?.Execute();
                    CompleteUI();
                    
                    }
                ));

                actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel,null));

               

                // Display the alert
                this.PresentViewController(actionSheetAlert, true, null);
            });
        }
        
        private void ApplyBinding()
        {
            var set = this.CreateBindingSet<AddNoteView, AddNoteViewModel>();
            set.Bind(ConfirmButton).To(vm => vm.ConfirmCommand);
            set.Bind(CancelButton).To(vm => vm.BackCommand);
            set.Bind(HeaderText).To(vm => vm.Header);
            set.Bind(ContentText).To(vm => vm.Content);
            set.Bind(this).For(v=>v.CompletedCommand).To(vm => vm.CompletedCommand);
            set.Bind(this).For(v => v.IsDone).To(vm => vm.IsDone).OneWay();
            set.Bind(this).For(v => v.IsEditing).To(vm => vm.IsEditing).OneWay();
            
            set.Apply();
        }

        private void Do(UIAlertAction action)
        {
            var content = new UNMutableNotificationContent();
            content.Title = "You have to do an important thing";
            content.Subtitle = HeaderText.Text;
            content.Body = ContentText.Text;
            content.Badge = 1;

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(2, false);

            var requestID = "sampleRequest";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);
            

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => { });
        }

        private void CompleteUI()
        {
            if(!IsDone)CompletedLabel.TextColor = UIColor.Red; else CompletedLabel.TextColor = UIColor.Green;
            if (!IsDone) CompletedLabel.Text = "Not completed"; else CompletedLabel.Text = "Completed";
        }
    }
}

