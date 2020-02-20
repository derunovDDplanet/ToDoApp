using System;
using ToDo.Core.Models;

namespace ToDo.Core.Services
{
    public class DialogActionInfo
    {
        public DialogActionInfo(string title, Action<Note> action=null)
        {
            Title = title;
            Action = action;
        }

        public string Title { get; private set; }
        public Action<Note> Action { get; set; }
        public bool IsCancel { get; set; } = false;
    }
}
