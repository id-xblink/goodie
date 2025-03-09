using Microsoft.EntityFrameworkCore;
using GoodsAndOrders.Data;
using GoodsAndOrders.UnitOfWork;
using GoodsAndOrders.Repositories;
using GoodsAndOrders.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GoodsAndOrders.Utils;
using GoodsAndOrders.Abstractions.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();


// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention()
        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

//// Регистрация репозиториев отдельно
//builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
//builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();

// Регистрации репозиториев в целом
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<OrderService>();

//builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Запускаем инициализацию базы данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // Применяем миграции
    DbInitializer.Initialize(context); // Вызываем инициализатор
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.MapGet("/", () => "API работает!");

app.Run();
