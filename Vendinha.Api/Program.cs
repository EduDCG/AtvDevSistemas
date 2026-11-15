using Vendinha.Contexts;
using Vendinha.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "http://localhost:4173",
                "http://127.0.0.1:4173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// configurando os serviços da aplicação
builder.Services.AddScoped<ClienteService>();

var connstring = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<VendinhaDbContext>(o => o.UseNpgsql(connstring));

var app = builder.Build();


// PIPELINES
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");
app.MapControllers();

app.Run();