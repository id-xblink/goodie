using GoodsAndOrders.Data.Entities;
using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using Microsoft.Extensions.Configuration;


namespace GoodsAndOrders.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<ProductUserOrder> ProductsUserOrders { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserOrder> UserOrders { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
                optionsBuilder.UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Явное указание имен таблиц (если без атрибута [Table])
            modelBuilder.Entity<OrderStatus>().ToTable("order_statuses");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<ProductCategory>().ToTable("product_categories");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<UserRole>().ToTable("user_roles");
            modelBuilder.Entity<UserOrder>().ToTable("user_orders");
            modelBuilder.Entity<UserOrder>().Property(o => o.OrderNumber).UseIdentityColumn();
            modelBuilder.Entity<ProductUserOrder>().ToTable("products_user_orders");
            modelBuilder.Entity<ProductUserOrder>()
                    .HasOne(puo => puo.UserOrder)
                    .WithMany(o => o.OrderElements)
                    .HasForeignKey(puo => puo.UserOrderId)
                    .OnDelete(DeleteBehavior.Cascade); // ❌ Запрещаем каскадное удаление!

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserOrders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<>().ToTable("");


            // Настройка связи User → UserRole (Один ко многим)
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRole)  // Один пользователь имеет одну роль
                .WithMany(r => r.Users)   // Одна роль может быть у многих пользователей
                .HasForeignKey(u => u.UserRoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
