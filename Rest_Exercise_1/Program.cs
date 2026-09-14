using Rest_Exercise_1.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Define a policy name
var myAllowSpecificOrigins = "AllowAll";

// 2. Add CORS service and configure policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            //policy.AllowAnyOrigin()   // Allow any origin to access
            policy.WithOrigins("http://localhost:5173") // Allow specific origin
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllers();

// Register repository interfaces so controllers depend on abstractions.
// Keep them as singletons so the in-memory lists persist across requests.
builder.Services.AddSingleton<ICatRepository, CatsRepository>();
builder.Services.AddSingleton<IPersonRepository, PersonsRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 3. Define that CORS should be used!
app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
