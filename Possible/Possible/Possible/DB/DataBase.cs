using Possible.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Possible.DB
{
    public class DataBase
    {
        readonly SQLiteAsyncConnection _database;

        public DataBase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Item>().Wait();
            _database.CreateTableAsync<Assignment>().Wait();
        }

        public Task<List<Item>> GetItensAsync()
        {
            return _database.Table<Item>().ToListAsync();
        }

        public Task<Item> GetItemAsync(int id)
        {
            return _database.Table<Item>()
                            .Where(i => i.ItemID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Item item)
        {
            if (item.ItemID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Item item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
