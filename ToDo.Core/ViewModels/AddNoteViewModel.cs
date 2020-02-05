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
        

        public AddNoteViewModel(IMvxNavigationService mvxNavigationService)
        {
            _navigationService = mvxNavigationService;
            
        }

        public IMvxCommand BackCommand => new MvxAsyncCommand(async () => await _navigationService.Close(this, new Result() { IsRejected = true }));
        public IMvxCommand ConfirmCommand => new MvxAsyncCommand(ConfirmExecute);



        public Note Note { get; set; }
        public bool IsEditing { get; set; }

        public string Header { get; set; }
        
        public string Content { get; set; }
        

        public override void Prepare(Note parameter)
        {
            Note = parameter;
            Header = parameter.Header;
            Content = parameter.Content;
            IsEditing = true;
        }

        

        

        private async Task ConfirmExecute()
        {
            if(IsEditing)
            {
                Note.Header = Header;
                Note.Content = Content;
            }
            else
            {
                Note = new Note(Header, Content);
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
