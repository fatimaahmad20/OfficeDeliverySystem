namespace deliverySystem.Models
{
    using deliverySystem.Models;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class OfficeDeliveryContext : DbContext
    {

        public OfficeDeliveryContext() : base("name=OfficeDeliveryConnectionString")
        {
            Database.SetInitializer(new OfficeDeliverySystemInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}