using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Infa;

public class GroceryDatabase(DataOptions<GroceryDatabase> dataopts) : DataConnection(dataopts.Options)
{
    public ITable<GroceryItem> Groceries() => this.GetTable<GroceryItem>();
}

