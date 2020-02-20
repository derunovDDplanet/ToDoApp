using System;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SQLite;
namespace ToDo.Core.Models
{
    [Table("Notes")]
    public class Note : MvxNotifyPropertyChanged
    {
        private string _header;
        private string _content;
        private bool _isDone;

        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }


        public Note()
        {
            _isDone = false;
        }

        public Note(string header, string content, bool isDone =false)
        {
            Header = header;
            Content = content;
            IsDone = isDone;
        }

        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        public bool IsDone
        {
            get => _isDone;
            set => SetProperty(ref _isDone, value);
        }

        public override string ToString()
        {
            return Header;
        }

    }
}
