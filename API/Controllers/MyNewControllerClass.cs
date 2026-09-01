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
    public List<MyEntitiy> GetEntityByIdOrThrowException([FromQuery] int? minPrice)
    {
        var guid = Guid.NewGuid().ToString();
        Console.WriteLine(guid);

        //Validering
        if (minPrice != null && minPrice < 0)
            throw new ValidationException("Min price cannot be less than 0");

        //Lav om til IQueryable
        var query = Entities.AsQueryable();

        //Filter
        query = query
            .Where(myEntity => myEntity.Id == "id 1" || myEntity.Id.Contains("id"))
            .Where(myEntity => myEntity.SomeNumber < 42);

        //Sortering
        query = query.OrderByDescending(g => g.SomeNumber);

        //Udfør query gennem .ToList()
        return query.ToList();
    }
}