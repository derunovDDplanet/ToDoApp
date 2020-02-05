using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using ToDo.Core;
using UIKit;


namespace ToDo.iOS.Views
{
    public partial class AddNoteView : MvxViewController<AddNoteViewModel>
    {
        public AddNoteView() : base("AddNoteView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ApplyBinding();
        }

        private void ApplyBinding()
        {
            var set = this.CreateBindingSet<AddNoteView, AddNoteViewModel>();
            set.Bind(ConfirmButton).To(vm => vm.ConfirmCommand);
            set.Bind(CancelButton).To(vm => vm.BackCommand);
            set.Bind(HeaderText).To(vm => vm.Header);
            set.Bind(ContentText).To(vm => vm.Content);
            set.Apply();
        }
    }
}

