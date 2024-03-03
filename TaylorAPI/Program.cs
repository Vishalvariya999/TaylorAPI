using Application.Commands.Roles;
using Domain.Data;
using MediatR;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using TaylorAPI.Config;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/* AutoMapping, EdmBuilder, ConfigurationSection */
//builder.Services.AddConfigurationSectionBindings(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapping));

/* HTTP_CONTEXT_ACCESSOR */
builder.Services.AddHttpContextAccessor();

/* Data base connection string */
builder.Services.AddDbContext<TaylorDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString(nameof(TaylorDBContext));
    options.UseSqlServer(connectionString);
});

/* CORS */
builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddPolicy("default", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

/* EDM Model */
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
        options.SerializerSettings.Formatting = Formatting.None;
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    })
    .AddOData(options =>
    {
        options.TimeZone = TimeZoneInfo.Utc;
        options.Count().Filter().Expand().Select().OrderBy().SetMaxTop(200)
            .AddRouteComponents("odata", EdmModelBuilder.GetEdmModel());
    });

builder.Services.AddMediatR(typeof(GetRoleQuery).GetTypeInfo().Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

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
