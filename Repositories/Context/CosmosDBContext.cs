using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context
{
    public class CosmosDBContext : DbContext
    {
        public DbSet<UserContext> UserContexts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }


        //DONT FORGET TO NUGET INSTALL: Install-Package Microsoft.EntityFrameworkCore.Cosmos -Version 5.0.10 for DbContextbuilder Use Cosmos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "KiCoKalenderDB");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Containers");

            modelBuilder.Entity<UserContext>()
                .ToContainer("userContexts");

            modelBuilder.Entity<UserContext>()
                .HasNoDiscriminator();

            modelBuilder.Entity<UserContext>()
                .HasPartitionKey(u => u.PartitionKey);

            modelBuilder.Entity<UserContext>()
                .UseETagConcurrency();

            modelBuilder.Entity<Appointment>()
                .ToContainer("appointments");

            modelBuilder.Entity<Appointment>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Appointment>()
                .HasPartitionKey(ap => ap.PartitionKey);

            modelBuilder.Entity<Appointment>()
                .UseETagConcurrency();

            modelBuilder.Entity<Asset>()
                .ToContainer("assets");

            modelBuilder.Entity<Asset>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Asset>()
                .HasPartitionKey(a => a.PartitionKey);

            modelBuilder.Entity<Asset>()
                .UseETagConcurrency();

            modelBuilder.Entity<Family>()
                .ToContainer("families");

            modelBuilder.Entity<Family>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Family>()
                .HasPartitionKey(f => f.PartitionKey);

            modelBuilder.Entity<Family>()
                .UseETagConcurrency();

            modelBuilder.Entity<Address>()
                .ToContainer("addresses");

            modelBuilder.Entity<Address>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Address>()
                .HasPartitionKey(ad => ad.PartitionKey);

            modelBuilder.Entity<Address>()
                .UseETagConcurrency();

            modelBuilder.Entity<User>()
                .ToContainer("users");

            modelBuilder.Entity<User>()
                .HasNoDiscriminator();

            modelBuilder.Entity<User>()
                .HasPartitionKey(us => us.PartitionKey);

            modelBuilder.Entity<User>()
                .UseETagConcurrency();
        }
    }
}
