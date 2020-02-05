using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ToDo.Core.Models;

namespace ToDo.Core
{
    public class ToDoListViewModel : MvxViewModel
    {
        private MvxObservableCollection<Note> _notes;
        private readonly IMvxNavigationService _navigationService;
        
      

        public ToDoListViewModel(IMvxNavigationService mvxNavigationService)
        {
            _navigationService = mvxNavigationService;
            NoteSelectedCommand = new MvxAsyncCommand<Note>(NoteSelected);
            Notes = new MvxObservableCollection<Note>
            {
                new Note("Warning", "Oh my god"),
                new Note("asd", "Oh my god 2"),
                new Note("zxc", "Oh my god 3")
            };
            
        }

        public IMvxCommand AddNoteCommand => new MvxAsyncCommand(AddNoteExecute);
        public IMvxCommand<Note> NoteSelectedCommand { get; private set; }
        public IMvxCommand<Note> RemoveFromToDoListCommand => new MvxAsyncCommand<Note>(RemoveFromToDoListExecute);


        public MvxObservableCollection<Note> Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private async Task NoteSelected(Note Note)
        {
            var result = await _navigationService.Navigate<AddNoteViewModel,Note,AddNoteViewModel.Result>(Note);
            if (result.IsRejected || result.note == null)
                return;
            await RaisePropertyChanged(nameof(Notes));
        }

        private async Task AddNoteExecute()
        {
            var result = await _navigationService.Navigate<AddNoteViewModel, AddNoteViewModel.Result>();

            if (result.IsRejected || result.note == null)
                return;
            if(string.IsNullOrWhiteSpace(result.note.Header))
            {
                result.note.Header = "Без названия";
            }

            Notes.Add(result.note);
            await RaisePropertyChanged(nameof(Notes));
        }

        private async Task RemoveFromToDoListExecute(Note note)
        {
            Notes.Remove(note);
            await RaisePropertyChanged(nameof(Notes));
        }

    }
}

