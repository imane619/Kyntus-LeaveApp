
using Microsoft.EntityFrameworkCore;
using LeaveService.Data;
using LeaveService.Services;
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// . Connexion PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// . Injection des Services (SOLID)
builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<ILeaveProcessingService, LeaveProcessingService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// . Définir la politique


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// . Utiliser la politique


app.UseCors("AllowAngular");

app.MapControllers();

// Endpoint de test rapide
app.MapGet("/test-leave", async (ILeaveProcessingService leaveProc) =>
{
    await leaveProc.ValidateLeaveAsync(Guid.NewGuid(), "EMP001", DateTime.Now, DateTime.Now.AddDays(2));
    return Results.Ok("C'est envoyé !");
});

app.Run();