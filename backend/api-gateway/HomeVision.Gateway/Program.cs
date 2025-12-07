using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder (args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient for calling other microservices
builder.Services.AddHttpClient();

// SignalR for real-time communication
builder.Services.AddSignalR();

// CORS configuration
var corsOrigins = builder.Configuration["CorsOrigins"]?.Split(',') 
    ?? new[] { "http://localhost:3000", "http://localhost:8080" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// SignalR hub endpoint
// app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
