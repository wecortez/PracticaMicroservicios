using GestionUsuarios.Api.Data;
using GestionUsuarios.Api.Interfaces;
using GestionUsuarios.Api.Repositorios;
using GestionUsuarios.Api.Servicios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BdGestionUsuariosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionGestionUsuarios")));

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();