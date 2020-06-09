using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.BusinessTests
{
    public class TestHelper
    {
        public static ApplicationDbContext GetContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            var dbContextOptions = builder.Options;
            var context = new ApplicationDbContext(dbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
