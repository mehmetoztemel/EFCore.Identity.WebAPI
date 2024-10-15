using EFCore.Identity.WebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Identity.WebAPI.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles");
        //builder.Entity<IdentityUserRole<Guid>>().HasKey(ur => new { ur.UserId, ur.RoleId });
        //builder.Ignore<IdentityUserLogin<Guid>>();
        //builder.Ignore<IdentityUserToken<Guid>>();
        //builder.Ignore<IdentityUserClaim<Guid>>();
        //builder.Ignore<IdentityRoleClaim<Guid>>();
        //}

        //public DbSet<AppUser> AppUsers { get; set; }
        //public DbSet<AppRole> AppRoles { get; set; }
    }
}