using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace ToDo.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
          
            RegisterCustomAppStart<AppStart>();
        }
    }
}
