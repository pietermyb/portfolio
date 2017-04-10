using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Portfolio.Data.Context;
using Portfolio.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Portfolio.Data
{
    public class IdentityDbInitializer
    {
        private PortfolioIdentityContext _context;
        
        public IdentityDbInitializer(IServiceProvider serviceProvider)
        {
            _context = (PortfolioIdentityContext)serviceProvider.GetService(typeof(PortfolioIdentityContext));
        }

        public void Seed()
        {
            var user = new PortfolioIdentityUser
            {
                UserName = "pietermyb",
                Email = "pietermyb@gmail.com",
                Name = "Pieter",
                Surname = "Myburgh",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(_context);

            IdentityRole adminRole = _context.Roles.FirstOrDefault(r => r.Name == "Admin");

            if (adminRole == null)
            {
                adminRole = new IdentityRole { Name = "Admin", NormalizedName = "Admin" };
                _context.Roles.Add(adminRole);
            }

            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<PortfolioIdentityUser>();
                var hashed = password.HashPassword(user, "P@m180584");
                user.PasswordHash = hashed;

                var userStore = new UserStore<PortfolioIdentityUser>(_context);
                var claim = new Claim("SuperUser", "True");
                
                userStore.AddToRoleAsync(user, "Admin");
                userStore.AddClaimsAsync(user,new List<Claim> { claim });
                _context.Users.Add(user);
               

            }

            _context.SaveChanges();
        }
        
    }
}
