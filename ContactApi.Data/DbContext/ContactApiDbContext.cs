using ContactApi.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactApi.Data.DbContext
{
    public class ContactApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ContactApiDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<Contact>().HasKey(x => x.Id);
            builder.Entity<Contact>().Property(x => x.FullName).IsRequired();
        }
    }
}