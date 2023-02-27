using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;
using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .Property(e => e.Deposit);
            var adminRoleId = Guid.NewGuid().ToString();
            var buyerRoleId = Guid.NewGuid().ToString();
            var sellerRoleid = Guid.NewGuid().ToString();
            builder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole {
                    Id = buyerRoleId,
                    Name = "Buyer",
                    NormalizedName = "BUYER"
                },
                new IdentityRole {
                    Id = sellerRoleid,
                    Name = "Seller",
                    NormalizedName = "SELLER"
                },
                new IdentityRole {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
            });

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<AppUser>();

            var adminUserId = Guid.NewGuid().ToString();
            //Seeding the User to AspNetUsers table
            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = adminUserId, // primary key
                    UserName = "ferencz",
                    NormalizedUserName = "FERENCZ",
                    PasswordHash = hasher.HashPassword(null, "admin"),
                    Deposit = 100
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = buyerRoleId,
                    UserId = adminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = sellerRoleid,
                    UserId = adminUserId
                }
            );
        }
    }
}