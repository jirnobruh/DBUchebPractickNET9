using Microsoft.EntityFrameworkCore;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Technic> Technics { get; set; }
        public DbSet<TechnicCategory> TechnicCategories { get; set; }
        public DbSet<TechnicStatus> TechnicStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<DeliveryOption> DeliveryOptions { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<TechnicalService> TechnicalServices { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // USERS
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.UserID);

                entity.Property(e => e.UserID).HasColumnName("userid");
                entity.Property(e => e.FirstName).HasColumnName("firstname");
                entity.Property(e => e.LastName).HasColumnName("lastname");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
                entity.Property(e => e.IdRole).HasColumnName("idrole");

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.IdRole);
            });

            // ROLES
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(e => e.RoleID);
                entity.Property(e => e.RoleID).HasColumnName("roleid");
                entity.Property(e => e.RoleTitle).HasColumnName("roletitle");
            });

            // TECHNIC
            modelBuilder.Entity<Technic>(entity =>
            {
                entity.ToTable("technic");

                entity.HasKey(e => e.TechnicID);
                entity.Property(e => e.TechnicID).HasColumnName("technicid");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.IdCategory).HasColumnName("idcategory");
                entity.Property(e => e.IdStatus).HasColumnName("idstatus");
                entity.Property(e => e.PricePerDay).HasColumnName("priceperday");

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Technics)
                      .HasForeignKey(e => e.IdCategory);

                entity.HasOne(e => e.Status)
                      .WithMany(s => s.Technics)
                      .HasForeignKey(e => e.IdStatus);
            });

            // TECHNIC CATEGORY
            modelBuilder.Entity<TechnicCategory>(entity =>
            {
                entity.ToTable("techniccategory");

                entity.HasKey(e => e.TechnicCategoryID);
                entity.Property(e => e.TechnicCategoryID).HasColumnName("techniccategoryid");
                entity.Property(e => e.CategoryTitle).HasColumnName("categorytitle");
                entity.Property(e => e.Description).HasColumnName("description");
            });

            // TECHNIC STATUS
            modelBuilder.Entity<TechnicStatus>(entity =>
            {
                entity.ToTable("technicstatus");

                entity.HasKey(e => e.TechnicStatusID);
                entity.Property(e => e.TechnicStatusID).HasColumnName("technicstatusid");
                entity.Property(e => e.StatusTitle).HasColumnName("statustitle");
            });

            // DELIVERY OPTIONS
            modelBuilder.Entity<DeliveryOption>(entity =>
            {
                entity.ToTable("deliveryoptions");

                entity.HasKey(e => e.DeliveryOptionID);
                entity.Property(e => e.DeliveryOptionID).HasColumnName("deliveryoptionid");
                entity.Property(e => e.OptionTitle).HasColumnName("optiontitle");
                entity.Property(e => e.Description).HasColumnName("description");
            });

            // ORDER STATUS
            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("orderstatus");

                entity.HasKey(e => e.OrderStatusID);
                entity.Property(e => e.OrderStatusID).HasColumnName("orderstatusid");
                entity.Property(e => e.StatusTitle).HasColumnName("statustitle");
            });

            // ORDERS
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.OrderID).HasColumnName("orderid");
                entity.Property(e => e.OrderDate).HasColumnName("orderdate");
                entity.Property(e => e.StartDate).HasColumnName("startdate");
                entity.Property(e => e.EndDate).HasColumnName("enddate");
                entity.Property(e => e.IdUser).HasColumnName("iduser");
                entity.Property(e => e.IdDeliveryOption).HasColumnName("iddeliveryoption");
                entity.Property(e => e.IdOrderStatus).HasColumnName("idorderstatus");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.IdUser);

                entity.HasOne(e => e.DeliveryOption)
                      .WithMany(d => d.Orders)
                      .HasForeignKey(e => e.IdDeliveryOption);

                entity.HasOne(e => e.OrderStatus)
                      .WithMany(s => s.Orders)
                      .HasForeignKey(e => e.IdOrderStatus);
            });

            // ORDER ITEMS
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("orderitems");

                entity.HasKey(e => e.OrderItemID);
                entity.Property(e => e.OrderItemID).HasColumnName("orderitemid");
                entity.Property(e => e.IdOrder).HasColumnName("idorder");
                entity.Property(e => e.IdTechnic).HasColumnName("idtechnic");
                entity.Property(e => e.PricePerDay).HasColumnName("priceperday");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.IdOrder);

                entity.HasOne(e => e.Technic)
                      .WithMany(t => t.OrderItems)
                      .HasForeignKey(e => e.IdTechnic);
            });

            // TECHNICAL SERVICE
            modelBuilder.Entity<TechnicalService>(entity =>
            {
                entity.ToTable("technicalservices");

                entity.HasKey(e => e.TechnicalServiceID);
                entity.Property(e => e.TechnicalServiceID).HasColumnName("technicalserviceid");
                entity.Property(e => e.IdTechnic).HasColumnName("idtechnic");
                entity.Property(e => e.TsDate).HasColumnName("tsdate");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.IdPerformedUser).HasColumnName("idperformeduser");

                entity.HasOne(e => e.Technic)
                      .WithMany(t => t.TechnicalServices)
                      .HasForeignKey(e => e.IdTechnic);

                entity.HasOne(e => e.PerformedUser)
                      .WithMany(u => u.TechnicalServices)
                      .HasForeignKey(e => e.IdPerformedUser);
            });

            // CART
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.HasKey(e => e.CartID);
                entity.Property(e => e.CartID).HasColumnName("cartid");
                entity.Property(e => e.IdUser).HasColumnName("iduser");

                entity.HasOne(e => e.User)
                      .WithOne()
                      .HasForeignKey<Cart>(e => e.IdUser);
            });

            // CART ITEMS
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("cartitems");

                entity.HasKey(e => e.CartItemID);
                entity.Property(e => e.CartItemID).HasColumnName("cartitemid");
                entity.Property(e => e.IdCart).HasColumnName("idcart");
                entity.Property(e => e.IdTechnic).HasColumnName("idtechnic");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(e => e.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(e => e.IdCart);

                entity.HasOne(e => e.Technic)
                      .WithMany(t => t.CartItems)
                      .HasForeignKey(e => e.IdTechnic);
            });
        }
    }
}