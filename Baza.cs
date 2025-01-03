using System;
using System.Data.Entity;
using System.Linq;

namespace WindowsFormsApp1
{
    internal class Baza : DbContext
    {
        public Baza() : base("connection_Hotel_Lucia")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<Baza>());
        }

        public DbSet<Customers> Customers { get; set; }
    }
}
