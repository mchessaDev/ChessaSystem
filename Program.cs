using ChessaSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar o AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Registrar o FuncionarioRepository como um serviço
builder.Services.AddScoped<FuncionarioRepository>();

builder.Services.AddControllersWithViews();
var app = builder.Build();

// ✅ Adiciona suporte a arquivos estáticos (CSS, JS, Imagens)
app.UseStaticFiles();  // Permite o uso de arquivos estáticos como CSS, JS, imagens, etc.
app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();
