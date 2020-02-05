using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Views;
using ToDo.Core;
using ToDo.iOS.Sources;
using ToDo.iOS.Views.Cells;
using UIKit;

namespace ToDo.iOS.Views
{
    public partial class ToDoListView : MvxViewController<ToDoListViewModel>
    {
        public ToDoListView() : base("ToDoListView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ApplyBinding();
        }

        private void ApplyBinding()
        {
            var set = this.CreateBindingSet<ToDoListView, ToDoListViewModel>();
            
            var source = new NotesTableViewSource(TableView);
            


            set.Bind(source).For(v => v.ItemsSource).To(vm => vm.Notes).OneWay();
            set.Bind(source).For(v => v.SelectionChangedCommand).To(vm => vm.NoteSelectedCommand);
            set.Bind(AddNoteButton).To(vm => vm.AddNoteCommand);
            set.Bind(source).For(s => s.ItemRemoveCommand).To(v => v.RemoveFromToDoListCommand);
            
            set.Apply();

            TableView.Source = source;
            TableView.ReloadData();


        }
    }
}

