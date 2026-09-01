using API.Controllers;
using Infa;
using LinqToDB;

namespace API.Testing;

public sealed class GroceryTestHost : IDisposable
{
    private readonly string _path = Path.Combine(Path.GetTempPath(), $"grocery-tests-{Guid.NewGuid():N}.db");
    private readonly IServiceScope _scope;

    public GroceryTestHost()
    {
        var options = new DataOptions().UseSQLite($"Data Source={_path};Pooling=False");

        var services = new ServiceCollection();
        services.AddSingleton(new DataOptions<GroceryDatabase>(options));
        services.AddScoped<GroceryDatabase>();
        services.AddScoped<GroceriesController>();

        _scope = services.BuildServiceProvider().CreateScope();
        GrocerySeed.EnsureSeeded(Db);
    }

    public GroceryDatabase Db => _scope.ServiceProvider.GetRequiredService<GroceryDatabase>();
    public GroceriesController Controller => _scope.ServiceProvider.GetRequiredService<GroceriesController>();

    public void Dispose()
    {
        _scope.Dispose();
        File.Delete(_path);
    }
}

public abstract class GroceryTest : IDisposable
{
    private readonly GroceryTestHost _host = new();

    protected GroceriesController Controller => _host.Controller;
    protected GroceryDatabase Db => _host.Db;

    protected ITable<GroceryItem> Rows => Db.Groceries();
    protected int RowCount => Db.Groceries().Count();
    protected GroceryItem Row(Guid id) => Db.Groceries().Single(x => x.Id == id);
    protected GroceryItem Row(string name) => Db.Groceries().Single(x => x.Name == name);
    protected bool RowExists(Guid id) => Db.Groceries().Any(x => x.Id == id);

    protected Guid InsertRow(GroceryItem item)
    {
        item.Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id;
        item.CreatedAtUtc = item.CreatedAtUtc == default ? DateTime.UtcNow : item.CreatedAtUtc;
        Db.Insert(item);
        return item.Id;
    }

    public void Dispose() => _host.Dispose();
}
