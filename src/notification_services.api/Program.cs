using Microsoft.EntityFrameworkCore;
using notification_services.application.Commands.Commands;
using notification_services.application.Commands.Handlers;
using notification_services.application.Interfaces;
using notification_services.domain.Interfaces;
using notification_services.infrastructure.Hubs;
using notification_services.infrastructure.Persistence.Context;
using notification_services.infrastructure.Persistence.Repositories;
using notification_services.infrastructure.RealTime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using notification_services.infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // A. Configura aquí tu URL de Keycloak
        // Ejemplo: "http://localhost:8080/realms/tu-realm"
        options.Authority = builder.Configuration["Jwt:Authority"] ?? "http://localhost:8080/realms/myrealm";
        options.RequireHttpsMetadata = false; // Pon true en producción

        // B. Configuración para validar el token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true, // O false si te da problemas al inicio
            ValidAudience = "account", // O el ClientId de tu Keycloak
            ValidateLifetime = true,

            // ¡IMPORTANTE! Esto hace que SignalR use el campo 'sub' como el ID del usuario
            NameClaimType = "sub"
        };

        // C. TRUCO PARA SIGNALR: Leer el token desde la URL (?access_token=...)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                // Si hay token y la ruta es la del Hub...
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

//  Configurar CORS (Crucial para que el Frontend conecte a SignalR)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// Add services to the container.

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true; 
});
//crear variable para la cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgre");
//registrar servicio para la conexion


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString,
        b => b.MigrationsAssembly("notification_service.infrastructure")));

// Registrar Capa Application (MediatR)

builder.Services.AddScoped<INotificationRepositoryPostgres, NotificationRepository>();
builder.Services.AddScoped<IRealTimeNotifier, SignalRNotifier>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

// Inyección de Dependencias (Vincular Interfaces con Implementaciones)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(SendPaymentNotificationCommand).Assembly,
    typeof(SendPaymentNotificationHandler).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowReactApp");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
