using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using ToDo.Core.Models;

namespace ToDo.Core.DataBase
{
    public class NoteAsyncRepository
    {
        SQLiteAsyncConnection _database;

        public NoteAsyncRepository(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath);
        }

        public async Task CreateTable()
        {
            await _database.CreateTableAsync<Note>();
        }
        public async Task<IEnumerable<Note>> GetItemsAsync()
        {
            return await _database.Table<Note>().ToListAsync();

        }
        public async Task<Note> GetItemAsync(int id)
        {
            return await _database.GetAsync<Note>(id);
        }
        public async Task<int> DeleteItemAsync(Note item)
        {
            return await _database.DeleteAsync(item);
        }
        public async Task<int> SaveItemAsync(Note item)
        {
            if (item.Id != 0)
            {
                await _database.UpdateAsync(item);
                return item.Id;
            }
            else
            {
                return await _database.InsertAsync(item);
            }
        }
    }
}