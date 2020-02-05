// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ToDo.iOS.Views
{
    [Register ("AddNoteView")]
    partial class AddNoteView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ConfirmButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView ContentText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField HeaderText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (ConfirmButton != null) {
                ConfirmButton.Dispose ();
                ConfirmButton = null;
            }

            if (ContentText != null) {
                ContentText.Dispose ();
                ContentText = null;
            }

            if (HeaderText != null) {
                HeaderText.Dispose ();
                HeaderText = null;
            }
        }
    }
}