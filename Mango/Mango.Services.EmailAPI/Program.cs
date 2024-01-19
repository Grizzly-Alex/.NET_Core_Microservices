using Mango.EmailAPI.Utility;
using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlServer<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

SD.EmailShoppingCart = builder.Configuration["EmailShoppingCartQueue:emailshoppingcart"];
SD.EmailShoppingCart = builder.Configuration["RegisterUserQueue:registeruser"];
SD.EmailShoppingCart = builder.Configuration["OrderCreatedTopic:OrderCreated"];
SD.EmailShoppingCart = builder.Configuration["OrderCreated_Email_Subscription:OrderCreatedEmail"];

builder.Services.AddControllers();
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

ApplyMigration();

app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Any())
        {
            _db.Database.Migrate();
        }
    }
}