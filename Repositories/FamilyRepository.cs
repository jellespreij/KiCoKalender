﻿using Context;
using Microsoft.Azure.Cosmos;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using User = Models.User;

namespace Repositories
{
    public class FamilyRepository : BaseRepository<Family>, IFamilyRepository
    {
        public FamilyRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext)
        {

        }

        public async void AddFolderToFamily(Folder folder, Guid id)
        {
            await _context.Database.EnsureCreatedAsync();

            var familyToUpdate = _context.Set<Family>().Where(entity => entity.Id == id).FirstOrDefault();
            familyToUpdate.Folders.Add(folder);

            Commit();
        }

        public async void AddUserToFamily(User user, Guid id)
        {
            await _context.Database.EnsureCreatedAsync();

            var familyToUpdate = _context.Set<Family>().Where(entity => entity.Id == id).FirstOrDefault();
            familyToUpdate.Users.Add(user);

            Commit();
        }
    }
}
