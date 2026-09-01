using Infa;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;



public class MyAmazingController(GroceryDatabase db) : ControllerBase
{


    [HttpGet(nameof(GetWhereTrue))]
    public List<MyEntity> GetWhereTrue()
    {
        var q = db.Entities().AsQueryable();
        
        //filtering
        q = q.Where(g => g.MyProp.Contains("Value"));

        q = q.OrderBy(g => g.Id)
            .ThenBy(o => o.MyProp);
        
        return q.ToList();
    }
}