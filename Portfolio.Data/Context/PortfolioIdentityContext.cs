using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Portfolio.Model.Entities;

namespace Portfolio.Data.Context
{
    /// <summary>
    /// Portfolio Identity Context
    /// </summary>
    public class PortfolioIdentityContext : IdentityDbContext<PortfolioIdentityUser, IdentityRole, string>
    {
        public PortfolioIdentityContext
           (DbContextOptions<PortfolioIdentityContext> options)
        : base(options)
        {
        }

        public DbSet<PortfolioIdentityUser> ApplicationIdentityRoleUsers { get; set; }

        public DbSet<IdentityRole> ApplicationIdentityRoleRoles { get; set; }

        /// <summary>
        /// Factory class for Application Identity Db Context
        /// </summary>
        public static class PortfolioIdentityContextFactory
        {
            public static PortfolioIdentityContext Create(string connectionString)
            {
                var optionsBuilder = new DbContextOptionsBuilder<PortfolioIdentityContext>();
                optionsBuilder.UseMySQL(connectionString);

                //Ensure database creation
                var context = new PortfolioIdentityContext(optionsBuilder.Options);
                context.Database.EnsureCreated();

                return context;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
