using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Binding.Views;
using ToDo.Core;
using ToDo.Core.Models;
using ToDo.iOS.Interfaces;
using UIKit;

namespace ToDo.iOS.Views.Cells
{
    public partial class MyTableViewCell : MvxTableViewCell, IPreparable
    {
        public static readonly NSString Key = new NSString("MyTableViewCell");
        public static readonly UINib Nib;
        public IMvxCommand ActionSheetCommand{get;set;}

        public bool IsPrepared { get; private set; }

        static MyTableViewCell()
        {
            Nib = UINib.FromName("MyTableViewCell", NSBundle.MainBundle);

        }

        protected MyTableViewCell(IntPtr handle) : base(handle)
        {

            
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<MyTableViewCell, Note>();
                set.Bind(Label).To(v => v.Header);
                set.Apply();
            });

        }
        
        private void CompleteUI()
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            var recognizer = new UITapGestureRecognizer(ActionSheetExecute);
            Button.AddGestureRecognizer(recognizer);
            Button.UserInteractionEnabled = true;
        }

        private void ActionSheetExecute()
        {
            ActionSheetCommand?.Execute(this.DataContext);
        }

        public void Prepare()
        {
            if (IsPrepared)
                return;

            IsPrepared = true;
            CompleteUI();
        }
    }
}
