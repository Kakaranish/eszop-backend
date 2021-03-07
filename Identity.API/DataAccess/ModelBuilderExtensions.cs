using Identity.API.Domain;
using Identity.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Common.Extensions;

namespace Identity.API.DataAccess
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder, IServiceProvider serviceProvider)
        {
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

            var hashedPassword = passwordHasher.Hash("Test1234");
            var user = new User("sa@mail.com", hashedPassword, Role.SuperAdmin);
            user.Bind(x => x.Id, Guid.NewGuid());
            modelBuilder.Entity<User>().HasData(user);
        }
    }
}
