using EFCommenter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

var options = new DbContextOptionsBuilder<DemoDbContext>()
    .UseSqlite("Data Source=demo.db") // دیتابیس SQLite
    .Options;

using var context = new DemoDbContext(options);
context.Database.EnsureCreated();

Console.WriteLine("Demo database created with comments.");

class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddEntitiesComments();
    }
}

class DemoDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
{
    public DemoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
        // It's only used for creating migrations; a real database doesn't need to exist.
        optionsBuilder.UseSqlServer("Server=.;Database=EfCommenterDemo;Trusted_Connection=True;TrustServerCertificate=True;");
        //optionsBuilder.UseSqlite("Data Source=demo.db");
        //optionsBuilder.UseNpgsql("Host=localhost;Database=efcommenter_demo;Username=postgres;Password=1234");

        return new DemoDbContext(optionsBuilder.Options);
    }
}


/// <summary>
/// The class declered a person!!!
/// </summary>
public class Person
{
    public int Id { get; set; }
    /// <summary>
    /// The full name of the person!!!
    /// </summary>
    public string Name { get; set; } = "";
    public PersonType Type { get; set; }
}

public enum PersonType
{
    Admin,
    User,
    Guest
}
