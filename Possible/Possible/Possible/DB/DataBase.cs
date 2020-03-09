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
            _database.CreateTableAsync<User>().Wait();
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


        public Task<List<Item>> GetItensByUserAsync(int userID)
        {
            return _database.Table<Item>()
                            .Where(i => i.UserID == userID)
                            .ToListAsync();
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

        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<User> GetUserAsync(int id)
        {
            return _database.Table<User>()
                            .Where(i => i.UserID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<User> GetUserAsync(string name, string password)
        {
            return _database.Table<User>()
                            .Where(i => i.Name.Equals(name) && i.Password.Equals(password))
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            if (user.UserID != 0)
            {
                return _database.UpdateAsync(user);
            }
            else
            {
                return _database.InsertAsync(user);
            }
        }

        public Task<int> DeleteUserAsync(User user)
        {
            return _database.DeleteAsync(user);
        }

        public Task<List<Assignment>> GetAssignmentsAsync()
        {
            return _database.Table<Assignment>().ToListAsync();
        }

        public Task<List<Assignment>> GetAssignmentsByItemAsync(int itemID)
        {
            return _database.Table<Assignment>()
                            .Where(assi => assi.ItemID == itemID)
                            .ToListAsync();

        }


       

        public Task<Assignment> GetAssignmentAsync(int id)
        {
            return _database.Table<Assignment>()
                            .Where(i => i.AssignmentID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAssignmentAsync(Assignment assignment)
        {
            if (assignment.AssignmentID != 0)
            {
                return _database.UpdateAsync(assignment);
            }
            else
            {
                return _database.InsertAsync(assignment);
            }
        }

        public Task<int> DeleteAssignmentAsync(Assignment assignment)
        {
            return _database.DeleteAsync(assignment);
        }

       
    }
}
