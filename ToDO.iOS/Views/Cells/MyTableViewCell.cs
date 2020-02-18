using System;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using ToDo.Core;
using ToDo.Core.Models;
using UIKit;

namespace ToDo.iOS.Views.Cells
{
    public partial class MyTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("MyTableViewCell");
        public static readonly UINib Nib;
     

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
                set.Bind(Button).To(v=>v.ActionSheetCommand);
                set.Apply();
            });

            CompleteUI();
        }
        
        private void CompleteUI()
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            
        }


    }
}
