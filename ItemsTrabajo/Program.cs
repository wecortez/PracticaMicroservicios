using ItemsTrabajo.Api.Data;
using ItemsTrabajo.Api.Interfaces;
using ItemsTrabajo.Api.Repositorios;
using ItemsTrabajo.Api.Servicios;
using Microsoft.EntityFrameworkCore;
using ItemsTrabajo.Api.ClientesHttp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BdItemsTrabajoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionItemsTrabajo")));

builder.Services.AddScoped<IItemTrabajoRepositorio, ItemTrabajoRepositorio>();
builder.Services.AddScoped<IItemTrabajoServicio, ItemTrabajoServicio>();

// Se registra el cliente HTTP para comunicarse con el microservicio de gestión de usuarios.
builder.Services.AddHttpClient<IUsuarioClienteHttp, UsuarioClienteHttp>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ServiciosExternos:GestionUsuariosUrl"]!
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();