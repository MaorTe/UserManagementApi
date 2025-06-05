using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserManagementApi.Data;
using UserManagementApi.Endpoints;
using UserManagementApi.Middleware;
using UserManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AutoShopDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarService, CarService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "User & Car Management API",
        Version = "v1"
    });
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontEnd", policy => {
        policy.WithOrigins("https://user-management-frontend-h8hc.onrender.com",
                           "http://localhost:5173",
                           "https://localhost:5173"
               )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Error handling
ExceptionHandler.Configure(app);

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AutoShopDbContext>();
    db.Database.Migrate();
}

app.UseCors("AllowFrontEnd");
app.MapUserEndpoints();
app.MapCarEndpoints();

app.Run();