using System;
using ToDo.Core.Models;

namespace ToDo.Core.Interfaces
{
    public interface IRemind
    {
        void Remind(Note note);
    }
}
