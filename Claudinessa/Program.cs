using Claudinessa.Data;
using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Data.Repositories.Orders.Repositories;
using Claudinessa.Data.Repositories.Orders.Repository;
using Claudinessa.Data.Repositories.Products.Interface;
using Claudinessa.Data.Repositories.Products.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var AllowSpecificOrigins = "Access-Control-Allow-Origin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: AllowSpecificOrigins,
        policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySqlConfig = new MySqlConfig(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton(mySqlConfig);

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IExtrasRepository, ExtrasRepository>();
builder.Services.AddScoped<IShipmentsRepository, ShipmentsRepository>();

var app = builder.Build();

app.UseCors(AllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
