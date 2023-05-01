using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProLi.Models;

namespace ProLi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProLi.Models.People> People { get; set; }
        public DbSet<ProLi.Models.Event> Event { get; set; }


        public DbSet<IdentityUserClaim<string>> IdentityUserClaim { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();
            modelBuilder.Ignore<ApplicationUser>();
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id });

            modelBuilder.Entity<Event>().
                HasMany(x => x.People)
                .WithMany(x => x.Events).UsingEntity(j => j.ToTable("EventPeople"));
         
        }

    }
}