using LoginSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoginSystemTests.Helpers
{
    public class InMemoryDatabaseHelper
    {
        private readonly DataContext _dataContext;
        public DataContext DataContext => _dataContext;
        public InMemoryDatabaseHelper()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "LoginSystemMemory");

            var dbContextOptions = builder.Options;
            _dataContext = new DataContext(dbContextOptions);
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
        }


    }
}
