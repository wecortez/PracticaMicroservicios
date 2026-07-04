using System;
using System.Collections.Generic;

namespace GestionUsuarios.Api.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public bool EstaActivo { get; set; }

    public DateTime FechaCreacion { get; set; }
}
