using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;
using System.Security.Claims;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace ECommerceAPI.Data
{
    public class AppDbContext:IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options,IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(p => p.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<Order>().Property(o => o.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<CartItem>().Property(ci => ci.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<ShippingAddress>().Property(sa => sa.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if(typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }
        }
        private static void SetSoftDeleteFilter<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                        (e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted));
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.CreatedBy = userId;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedAt = DateTime.UtcNow;
                        entity.UpdatedBy = userId;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entity.IsDeleted = true;
                        entity.DeletedAt = DateTime.UtcNow;
                        entity.UpdatedAt = DateTime.UtcNow;
                        entity.UpdatedBy = userId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
