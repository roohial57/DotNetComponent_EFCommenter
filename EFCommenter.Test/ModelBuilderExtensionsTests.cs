using EFCommenter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

public class ModelBuilderExtensionsTests
{

    [Fact(Skip = "Ignoring this test because it is incomplate")]
    public void AddEntitiesComments_ShouldAddComments_PostgreSQL()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql("Host=127.0.0.1;Database=EfCommenter_Test;Username=postgres;Trust Server Certificate=true")
            .Options;

        using var context = new TestDbContext(options);
        context.Database.EnsureCreated();

        // استفاده از design-time model
        var model = context.GetService<IDesignTimeModel>().Model;

        var entityType = model.GetEntityTypes().First(x => x.ClrType == typeof(Person));

        var nameComment = entityType.FindProperty("Name")?.GetComment();
        Assert.NotNull(nameComment);

        var typeComment = entityType.FindProperty("Type")?.GetComment();
        Assert.NotNull(typeComment);
    }


    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Person> People => Set<Person>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddEntitiesComments();
        }
    }


    /// <summary>
    /// The class declered a person!!!
    /// </summary>
    class Person
    {
        public int Id { get; set; }
        /// <summary>
        /// A comment to test!!!
        /// </summary>
        public string Name { get; set; } = "";
        public PersonType Type { get; set; }
    }

    enum PersonType
    {
        Admin,
        User,
        Guest
    }

}
