using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rest_Exercise_1.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add CORS service and configure multiple policies for educational purposes
builder.Services.AddCors(options =>
{
    // Restrictive: only the Vite dev server frontend, any method/header
    options.AddPolicy(name: "AllowFrontendOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });

    // Permissive: any origin, any method, any header (useful to demonstrate the risk)
    options.AddPolicy(name: "AllowAnyOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });

    // Restrictive: any origin, but only GET requests allowed
    options.AddPolicy(name: "AllowGetOnly",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .WithMethods("GET")
                  .AllowAnyHeader();
        });
});


// Add services to the container.
builder.Services.AddControllers();

// Add swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token as: Bearer {your_token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey, // Vi bruger ApiKey for at kunne skrive 'Bearer ' manuelt
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Kravet om at bruge ovenstående definition på endpoints
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});


// Register repository interfaces so controllers depend on abstractions.
// Keep them as singletons so the in-memory lists persist across requests.
builder.Services.AddSingleton<ICatRepository, CatsRepository>();
builder.Services.AddSingleton<IPersonRepository, PersonsRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


#region JWT Authentication Configuration
// Add JWT authentication services
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use swagger middleware to serve generated Swagger as a JSON endpoint and Swagger UI.
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 2. Define the DEFAULT CORS policy (used when no [EnableCors]/[DisableCors] attribute overrides it)
app.UseCors("AllowFrontendOrigin");

app.UseAuthentication(); // Checks "Who are you?"
app.UseAuthorization();  // Checks "Are you allowed to be here?"

app.MapControllers();

app.Run();
