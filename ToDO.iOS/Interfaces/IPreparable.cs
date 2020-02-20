using System;
namespace ToDo.iOS.Interfaces
{
    public interface IPreparable
    {
        bool IsPrepared { get; }

        void Prepare();
    }
}
