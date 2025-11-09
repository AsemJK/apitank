using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "ProductsCatalog_";
});
builder.Services.AddScoped<apitank.Services.IProductService, apitank.Services.ProductService>();

TypeAdapterConfig.GlobalSettings.Scan(typeof(apitank.Helpers.MapsterProfile).Assembly);
builder.Services.AddScoped<MapsterMapper.IMapper, MapsterMapper.Mapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || 1 == 0)
{
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
