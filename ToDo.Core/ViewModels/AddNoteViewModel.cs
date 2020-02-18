using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ToDo.Core.Models;

namespace ToDo.Core
{
    public class AddNoteViewModel : MvxViewModel<Note,AddNoteViewModel.Result>
    {
        readonly IMvxNavigationService _navigationService;
        private bool _isDone;
        private bool _isEditing;

        public AddNoteViewModel(IMvxNavigationService mvxNavigationService)
        {
            _navigationService = mvxNavigationService;
            IsDone = false;
        }

        public IMvxCommand BackCommand => new MvxAsyncCommand(async () => await _navigationService.Close(this, new Result() { IsRejected = true }));
        public IMvxCommand ConfirmCommand => new MvxAsyncCommand(ConfirmExecute);
        public IMvxCommand CompletedCommand => new MvxCommand(CompletedCommandExecute);
        


        public Note Note { get; set; }
        public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }
        public bool IsDone { get => _isDone; set => SetProperty(ref _isDone, value); }

        public string Header { get; set; }
        
        public string Content { get; set; }
        

        public override void Prepare(Note parameter)
        {
            Note = parameter;
            Header = parameter.Header;
            Content = parameter.Content;
            IsEditing = true;
            IsDone = parameter.IsDone;
        }


        private void CompletedCommandExecute()
        {
            IsDone = !IsDone;
            
        }

        private async Task ConfirmExecute()
        {
            if(IsEditing)
            {
                Note.Header = Header;
                Note.Content = Content;
                Note.IsDone = IsDone;
            }
            else
            {
                Note = new Note(Header, Content, IsDone);
            }

            await _navigationService.Close(this, new Result() { note = Note });

        }

        public class Result
        {
            public Note note { get; set; }
            public bool IsRejected { get; set; }
        }
    }
}
