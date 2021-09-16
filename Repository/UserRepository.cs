using KiCoKalender.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Repository
{
    class UserRepository
    {
        public CloudTable cloudTable;

        public UserRepository(CloudTable cloudTable) 
        {
            this.cloudTable = cloudTable;
        }

        public async Task<User> GetUserById(string userId) 
        {
            TableOperation tableOperation = TableOperation.Retrieve<User>("partitionKey", "RowKey");
            TableResult tableResult = await cloudTable.ExecuteAsync(tableOperation);
            return tableResult.Result as User;
        }

        public async void AddUser(User user) 
        {
            TableOperation tableOperation = TableOperation.Insert(user);
            TableResult result = await cloudTable.ExecuteAsync(tableOperation);
            User insertedUser = result.Result as User;
        }

        public async Task<User> GetUserByRole(Role role) 
        {
            TableOperation tableOperation = TableOperation.Retrieve<User>("partitionKey", "RowKey");
            TableResult tableResult = await cloudTable.ExecuteAsync(tableOperation);
            return tableResult.Result as User;
        }
    }
}
