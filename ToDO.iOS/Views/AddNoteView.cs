using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Plugin.Messenger;
using ToDo.Core;
using ToDo.Core.Services;
using UIKit;
using UserNotifications;


namespace ToDo.iOS.Views
{
    public partial class AddNoteView : MvxViewController<AddNoteViewModel>
    {
        private MvxSubscriptionToken _token;
        public ServiceBunch Service;

        private bool _isDone;
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                CompleteLabel();
            }
        }
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => _isEditing = value; 
        }

        public IMvxCommand CompletedCommand { get; set; }

        public AddNoteView() : base("AddNoteView", null)
        {

        }


        public override void ViewDidLoad()
        {
            
            base.ViewDidLoad();
            ApplyBinding();
            CompleteUI();
            _token = Service.Messenger.SubscribeOnMainThread<AlertDialogMessage>(ActionExecute);

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
            set.Bind(ActionsButton).To(vm => vm.ActionSheetCommand);
            
            set.Apply();
        }

        private void ActionExecute(AlertDialogMessage message)
        {

            // Create a new Alert Controller
            UIAlertController actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);

            // Add Actions
            if (message.Actions != null)
            {
                foreach (var actionInfo in message.Actions)
                {
                    UIAlertActionStyle style = actionInfo.IsCancel ? UIAlertActionStyle.Cancel : UIAlertActionStyle.Default;
                    var alertAction = UIAlertAction.Create(actionInfo.Title, style, a => actionInfo.Action?.Invoke(message.Note));
                    actionSheetAlert.AddAction(alertAction);

                }
            }


            // Display the alert
            this.PresentViewController(actionSheetAlert, true, null);

        }



        private void CompleteUI()
        {
            CompleteLabel();
        }

        private void CompleteLabel()
        {
            if (!IsDone) CompletedLabel.TextColor = UIColor.Red; else CompletedLabel.TextColor = UIColor.Green;
            if (!IsDone) CompletedLabel.Text = "Not completed"; else CompletedLabel.Text = "Completed";
        }
    }
}

