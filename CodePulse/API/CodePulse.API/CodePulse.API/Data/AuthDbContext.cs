using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "a95df4d0-1950-4f5a-ba2d-eb94a200719b";
            var writerRoleId = "bedd8267-b513-41f8-9423-29fe2aece3a4";

            //Create reader and writer Role
            var roles = new List<IdentityRole>
                {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                    ConcurrencyStamp=readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper(),
                    ConcurrencyStamp=writerRoleId
                }
            }; 

            //Sede the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an admin user and give roles to admin

            var adminUserId = "e24dfd15-5c12-4c9c-9eff-070f3f38c0f0";

            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            //Generate a password
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            //Seed the admin user in DB
            builder.Entity<IdentityUser>().HasData(admin);

            //Give both Roles to admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId=readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId=writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
