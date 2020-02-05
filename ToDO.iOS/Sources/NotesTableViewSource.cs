using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Binding.Views;
using ToDo.Core.Models;
using ToDo.iOS.Views.Cells;
using UIKit;

namespace ToDo.iOS.Sources
{
    public class NotesTableViewSource : MvxSimpleTableViewSource
    {
        

        public NotesTableViewSource(UITableView tableView) : base(tableView, typeof(MyTableViewCell))
        {
            DeselectAutomatically = true;
        }

        public IMvxCommand<Note> ItemRemoveCommand { get; set; }


        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = base.GetOrCreateCellFor(tableView, indexPath, item);

            return cell;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    var item = ItemsSource.ElementAt(indexPath.Row) as Note;
                    ItemRemoveCommand?.Execute(item);

                    break;
                case UITableViewCellEditingStyle.None:
                    break;
            }
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }

        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            return false;
        }
    }
}
