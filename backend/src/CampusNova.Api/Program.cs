using CampusNova.Api.Extensions;
using CampusNova.Api.Hubs;
using CampusNova.Api.Middlewares;
using CampusNova.Application;
using CampusNova.Infrastructure;
using CampusNova.Realtime.Providers;

var builder = WebApplication.CreateBuilder(args);

// Agregar soporte para Controllers
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddRealTime();
builder.Services.AddConfigurationInvalidatorValidation();
builder.Services.AddCorsConfiguration();
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

//app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
await app.WarmupAsync();
app.MapHub<NotificationHub>("/hubs/notifications");
// Mapear controllers
app.MapControllers();

app.Run();