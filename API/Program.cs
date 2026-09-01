using System.Text.Json.Serialization;
using API;
using Microsoft.AspNetCore.Mvc;
using Infa;
using LinqToDB;

var builder = WebApplication.CreateBuilder(args);

var options = new DataOptions().UseSQLite("Data Source=dev.db");
builder.Services.AddSingleton(new DataOptions<GroceryDatabase>(options));
builder.Services.AddScoped<GroceryDatabase>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApiDocument();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GroceryDatabase>();
    GrocerySeed.EnsureSeeded(db);
    db.CreateTable<MyEntity>(tableOptions:TableOptions.CreateIfNotExists);
    db.Insert(new MyEntity()
    {
        Id = "asælkdsa" + new Random().Next(),
        MyProp = "Value"
    });
    db.Insert(new MyEntity()
    {
        Id = "asælkdsa" + new Random().Next(),
        MyProp = "Not value"
    });
}

app.UseExceptionHandler(handler => handler.Run(async context =>
{
    var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
    context.Response.StatusCode = error switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        NotFoundException => StatusCodes.Status404NotFound,
        ConflictException => StatusCodes.Status409Conflict,
        NotImplementedException => StatusCodes.Status501NotImplemented,
        _ => StatusCodes.Status500InternalServerError
    };
    await context.Response.WriteAsJsonAsync(new ProblemDetails
    {
        Status = context.Response.StatusCode,
        Title = error?.GetType().Name,
        Detail = error?.Message
    });
}));
app.UseOpenApi();
app.UseSwaggerUi();
app.MapControllers();
app.Run();
