using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Baza : DbContext
    {
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        public Baza() : base("connection_Hotel_Lucia")
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (!_initialized)
                    {
                        Database.SetInitializer(new CreateDatabaseIfNotExists<Baza>());
                        Database.Initialize(false);
                        _initialized = true;
                    }
                }
            }
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Customers> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customers>()
                .ToTable("Customers")
                .HasKey(c => c.CustomersId);
        }
    }
}
