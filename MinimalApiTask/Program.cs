using Microsoft.EntityFrameworkCore;
using MinimalApiTask;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();
        
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<Employee>> GetAllEmployees(DataContext context) =>
    await context.Employees.ToListAsync();

app.MapGet("/", () =>"Welcome to the Employee DB");

app.MapGet("/Employee", async (DataContext context) =>
    await context.Employees.ToListAsync());
app.MapGet("/Employee/{id}",async(DataContext context,int id) =>
    await context.Employees.FindAsync(id) is Employee  name ?
    Results.Ok(name) :
    Results.NotFound("sorry,name not found.:/"));

app.MapPost("/Employee", async (DataContext context, Employee employee) =>
{
    context.Employees.Add(employee);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllEmployees(context));
});

app.MapPut("/Employee/{id}", async (DataContext context, Employee employee, int id) =>
{
    var dbEmployee = await context.Employees.FindAsync(id);
    if (dbEmployee == null) return Results.NotFound("No Employee found. :/");

    dbEmployee.FirstName = employee.FirstName;
    dbEmployee.LastName = employee.LastName;
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllEmployees(context));

});

app.MapDelete("/Employee/{id}", async (DataContext context, int id) =>
{
    var dbEmployee = await context.Employees.FindAsync(id);
    if (dbEmployee == null) return Results.NotFound("who's that?. :/");

    context.Employees.Remove(dbEmployee);
    await context.SaveChangesAsync();


    return Results.Ok(await GetAllEmployees(context));

});


app.Run();

