using System;
using MvvmCross.Plugin.Messenger;

namespace ToDo.Core.Services
{
    public struct  ServiceBunch
    {
        public IMvxMessenger Messenger => MvvmCross.Mvx.IoCProvider.Resolve<IMvxMessenger>();
    }
}
