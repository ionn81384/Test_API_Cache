using Exercicio_API.DTO;
using Exercicio_API.Persistence;
using Exercicio_API.TestData;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OData.Swagger.Services;

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<User>("Users");
    builder.EntitySet<Post>("Posts");
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .SetMaxTop(20)
        .Count()
        .Expand()
    );

builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase(databaseName: "TestDB"));
builder.Services.AddScoped<IAPIRepoPosts, APIRepoPosts>();
builder.Services.AddScoped<IAPIRepoUsers, APIRepoUsers>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add Seeder
DBSeeder.AddCompaniesData(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
