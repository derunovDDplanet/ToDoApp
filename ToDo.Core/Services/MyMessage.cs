using System;
using MvvmCross.Plugin.Messenger;
using ToDo.Core.Models;

namespace ToDo.Core.Services
{
    public class MyMessage : MvxMessage
    {
        public MyMessage(object sender):base(sender)
        {
            Note = sender as Note;
        }
        public Note Note { get; private set; }
    }
}
