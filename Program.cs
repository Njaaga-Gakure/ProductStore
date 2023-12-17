using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Extensions;
using ProductStore.Model.DTOs;
using ProductStore.Service;
using ProductStore.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configure con string
builder.Services.AddDbContext<ProductStoreContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));

// configure AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// register services for DI injection
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IProduct, ProductService>();
builder.Services.AddScoped<IOrder, OrderService>();
builder.Services.AddScoped<IJWT, JWTService>();
builder.Services.AddScoped<ResponseDTO, ResponseDTO>();

builder.AddSwaggenGenExtension();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAuth();
builder.AddAdminPolicy();

var app = builder.Build();

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
