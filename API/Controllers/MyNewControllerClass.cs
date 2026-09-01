using API;
using Infa;
using Microsoft.AspNetCore.Mvc;

public class MyEntitiy
{
    public string Id { get; set; }
    public string MyProperty { get; set; }
    public int SomeNumber { get; set; }
}


public class MyControllerClass(GroceryDatabase db) : ControllerBase
{
    public List<MyEntitiy> Entities = new List<MyEntitiy>()
    {
        new MyEntitiy() 
        { 
            Id = "id 1",
            MyProperty = "Value 1",
            SomeNumber = 42
        },
        new MyEntitiy() 
        { 
            Id = "id 1",
            MyProperty = "Value 1",
            SomeNumber = 43
        },
    };
    
    [HttpGet(nameof(GetEntityByIdOrThrowException))]
    public GroceryItem? GetEntityByIdOrThrowException([FromQuery]Guid id)
    {
        var q = db.Groceries().AsQueryable();

        var result = q
            .FirstOrDefault(g => g.Id == id);

        // if (result == null)
        //     throw new ValidationException("not found");
        //
        return result;
    }
}