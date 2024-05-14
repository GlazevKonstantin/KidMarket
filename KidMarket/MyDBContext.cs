using System.Data.Entity;

namespace KidMarket
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("connectionString") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserAll> UsersAll { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<TransportCompany> TransportCompanies { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Basket> Basket { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().ToTable("Managers");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<UserAll>().ToTable("UsersAll");
            modelBuilder.Entity<Sale>().ToTable("Sales");
            modelBuilder.Entity<TransportCompany>().ToTable("TransportCompanies");
            modelBuilder.Entity<Feedback>().ToTable("Feedbacks");
            modelBuilder.Entity<Basket>().ToTable("Basket");
        }
    }
}

