using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace SuddenlySleepy.Models
{
    public class AppIdentityDbContext : IdentityDbContext<SSUser>
    {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

        public DbSet<Donation> Donations { get; set; }
        public DbSet<SSEvent> SSEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("SSUsers");

            // Many to Many bridge between SSUser and SSEvent
            modelBuilder.Entity<SSUserSSEvent>()
                .HasKey(se => new { se.SSUserId, se.SSEventId}); // defines composite key
            modelBuilder.Entity<SSUserSSEvent>()
                .HasOne(se => se.SSUser)
                .WithMany(s => s.AttendedEvents)
                .HasForeignKey(se => se.SSUserId);
            modelBuilder.Entity<SSUserSSEvent>()
                .HasOne(se => se.SSEvent)
                .WithMany(e => e.RegisteredAttendees)
                .HasForeignKey(se => se.SSEventId);

            // Many to Many bridge between SSUser and Donation
            //modelBuilder.Entity<Donation>()
              //  .HasOne(d => d.Donor)
               // .WithMany(u => u.DonationsMade)
                //.IsRequired();
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            UserManager<SSUser> userManager =
                serviceProvider.GetRequiredService<UserManager<SSUser>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                SSUser user = new SSUser
                {
                    UserName = username,
                    Email = email
                };

                IdentityResult result = await userManager
                    .CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
