using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using ToDo.Core.Interfaces;
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

        public ServiceBunch Service;

        public  ToDoListViewModel(IMvxNavigationService mvxNavigationService)
        {
            _navigationService = mvxNavigationService;
            Notes = new MvxObservableCollection<Note>();

        }

      

        public IMvxCommand AddNoteCommand => new MvxAsyncCommand(AddNoteExecute);
        public IMvxCommand<Note> NoteSelectedCommand => new MvxAsyncCommand<Note>(NoteSelected);
        public IMvxCommand<Note> RemoveFromToDoListCommand => new MvxAsyncCommand<Note>(RemoveFromToDoListExecute);
        public IMvxCommand<Note> ActionSheetCommand => new MvxCommand<Note>(ActionSheetExecute);
        


       
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
            get => _searchResult;
            
            set => SetProperty(ref _searchResult, value);
        }

        public MvxObservableCollection<Note> Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }


        public async override void ViewCreated()
        {
            _notes = new MvxObservableCollection<Note>(await App.DataBase.GetItemsAsync());

            SearchResult = Notes;
            base.ViewCreated();
            
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
            if (string.IsNullOrWhiteSpace(result.note.Content))
            {
                result.note.Content = "";
            }
            Notes.Add(result.note);
            await RaisePropertyChanged(nameof(Notes));
            await RaisePropertyChanged(nameof(SearchResult));
            await App.DataBase.SaveItemAsync(result.note);
            
        }

        private async Task RemoveFromToDoListExecute(Note note)
        {
            Notes.Remove(note);
            SearchResult.Remove(note);
            await App.DataBase.DeleteItemAsync(note);
            
        }

        private void ActionSheetExecute(Note sender )
        {
            string CompletedLabel = sender.IsDone == true ? "Completed" : "Not completed";
            var dialogAction = new[]
            {
                new DialogActionInfo ("Remind After 5 minutes",(note)=>Mvx.IoCProvider.Resolve<IRemind>().Remind(note)),
                new DialogActionInfo (CompletedLabel,CompleteExecute),
                new DialogActionInfo ("Cancel") {IsCancel=true}
            };
            var showAcionDialogMessage = new AlertDialogMessage(sender)
            {
                Actions = dialogAction
            };

            Service.Messenger.Publish(showAcionDialogMessage);

        }

        public async void CompleteExecute(Note note)
        {
            note.IsDone = !note.IsDone;
            await App.DataBase.SaveItemAsync(note);
        }

        

    }
}

