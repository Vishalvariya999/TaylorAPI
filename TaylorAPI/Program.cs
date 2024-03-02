using Domain.Data;
using Microsoft.EntityFrameworkCore;
using TaylorAPI.Config;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/* Data base connection string */
builder.Services.AddDbContext<TaylorDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString(nameof(TaylorDBContext));
    options.UseSqlServer(connectionString);
});

/* AutoMapping, EdmBuilder, ConfigurationSection */
//builder.Services.AddConfigurationSectionBindings(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
