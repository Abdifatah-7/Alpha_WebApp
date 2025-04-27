using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Use the same connection string you have in appsettings.json
        optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Bibliotek\\Alpha\\Data\\Databases\\Database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
