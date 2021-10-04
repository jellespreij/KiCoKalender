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
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Family> Families { get; set; }


        //DONT FORGET TO NUGET INSTALL: Install-Package Microsoft.EntityFrameworkCore.Cosmos -Version 5.0.10 for DbContextbuilder Use Cosmos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "KiCoKalenderDB");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<List<long>, string>(
                v => string.Join(";", v),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(val => long.Parse(val)).ToList());


            modelBuilder.HasDefaultContainer("Containers");

            modelBuilder.Entity<User>()
                .ToContainer("users");

            modelBuilder.Entity<User>()
                .HasNoDiscriminator();

            modelBuilder.Entity<User>()
                .HasPartitionKey(o => o.PartitionKey);

            modelBuilder.Entity<User>()
                .UseETagConcurrency();

            modelBuilder.Entity<Appointment>()
                .ToContainer("appointments");

            modelBuilder.Entity<Appointment>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Appointment>()
                .HasPartitionKey(o => o.PartitionKey);

            modelBuilder.Entity<Appointment>()
                .UseETagConcurrency();

            modelBuilder.Entity<Asset>()
                .ToContainer("assets");

            modelBuilder.Entity<Asset>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Asset>()
                .HasPartitionKey(o => o.PartitionKey);

            modelBuilder.Entity<Asset>()
                .UseETagConcurrency();

            modelBuilder.Entity<Family>()
                .ToContainer("families");

            modelBuilder.Entity<Family>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Family>()
                .HasPartitionKey(o => o.PartitionKey);

            modelBuilder.Entity<Family>()
                .UseETagConcurrency();
        }
    }
}
