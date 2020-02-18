using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

using ToDo.Core.Models;
using ToDo.Core.Services;

namespace ToDo.Core
{
    public class ToDoListViewModel : MvxViewModel
    {
        
        private MvxObservableCollection<Note> _notes;
        private MvxObservableCollection<Note> _searchResult;

        private readonly IMvxNavigationService _navigationService;
        private  string _searchText;

        public IMvxMessenger Messenger => Mvx.IoCProvider.Resolve<IMvxMessenger>();

        public  ToDoListViewModel(IMvxNavigationService mvxNavigationService)
        {
            _navigationService = mvxNavigationService;
            NoteSelectedCommand = new MvxAsyncCommand<Note>(NoteSelected);
            Notes = new MvxObservableCollection<Note>();
        }

      

        public IMvxCommand AddNoteCommand => new MvxAsyncCommand(AddNoteExecute);
        public IMvxCommand<Note> NoteSelectedCommand { get; private set; }
        public IMvxCommand<Note> RemoveFromToDoListCommand => new MvxAsyncCommand<Note>(RemoveFromToDoListExecute);

        public IMvxCommand<Note> ActionCommand { get; set; }


       
        public string SearchText
        {
            get => _searchText;

            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    if(string.IsNullOrWhiteSpace(_searchText))
                    {
                        SearchResult = Notes;
                    }
                    else
                    {
                        SearchResult = new MvxObservableCollection<Note>(Notes.Where(n => n.Header.ToLower().Contains(_searchText.ToLower())));
                    }
                }
            }
        }

        public MvxObservableCollection<Note> SearchResult
        {
            get
            {
                return _searchResult;
            }
            set => SetProperty(ref _searchResult, value);
        }

        public MvxObservableCollection<Note> Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }


        public async override void ViewCreated()
        {
            Notes = new MvxObservableCollection<Note>(await App.DataBase.GetItemsAsync());
            foreach(var item in Notes)
            {
                item.ActionSheetEventHandler += ActionSheetExecute;
                
            }

            SearchResult = Notes;
            base.ViewCreated();
            
        }
        public override void ViewDestroy(bool viewFinishing = true)
        {
            foreach (var item in Notes)
            {
                item.ActionSheetEventHandler -= ActionSheetExecute;
            }
            base.ViewDestroy(viewFinishing);
        }

        public override void ViewAppearing()
        {
            SearchText = "";
            base.ViewAppearing();
          
        }

        private async Task NoteSelected(Note Note)
        {
            var result = await _navigationService.Navigate<AddNoteViewModel,Note,AddNoteViewModel.Result>(Note);
            if (result.IsRejected || result.note == null || result == null)
                return;
            await RaisePropertyChanged(nameof(Notes));
            
            await RaisePropertyChanged(nameof(SearchResult));
            await App.DataBase.SaveItemAsync(result.note);
        }

        private async Task AddNoteExecute()
        {
            var result = await _navigationService.Navigate<AddNoteViewModel, AddNoteViewModel.Result>();
            
            if (result.IsRejected || result.note == null )
                return;
            if(string.IsNullOrWhiteSpace(result.note.Header))
            {
                result.note.Header = "Без названия";
            }
            result.note.ActionSheetEventHandler += ActionSheetExecute;
            Notes.Add(result.note);
            await RaisePropertyChanged(nameof(Notes));
            await RaisePropertyChanged(nameof(SearchResult));
            await App.DataBase.SaveItemAsync(result.note);
            
        }

        private async Task RemoveFromToDoListExecute(Note note)
        {
            Notes.Remove(note);
            await RaisePropertyChanged(nameof(Notes));
            await RaisePropertyChanged(nameof(SearchResult));
            await App.DataBase.DeleteItemAsync(note);
            
        }

        private void ActionSheetExecute(object sender, EventArgs e)
        {

            var message = new MyMessage(sender);
            Messenger.Publish(message);
        }

        
    }
}

