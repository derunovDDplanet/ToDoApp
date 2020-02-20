using System;
using System.IO;
using System.Threading.Tasks;

using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using ToDo.Core.DataBase;

namespace ToDo.Core
{
    public class App : MvxApplication
    {
        public const string DATABASE_NAME = "Notes.db";
        public static NoteAsyncRepository _database;
        public static NoteAsyncRepository DataBase
        {
            get
            {
                if (_database == null)
                {
                    _database = new NoteAsyncRepository(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return _database;
            }
        }

        

        public async override void Initialize()
        {
            

            RegisterCustomAppStart<AppStart>();
            await DataBase.CreateTable();
        }

        
    }
}
