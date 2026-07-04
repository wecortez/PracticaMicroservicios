using System;
using System.Collections.Generic;

namespace ItemsTrabajo.Api.Models;

public partial class ItemTrabajo
{
    public int IdItemTrabajo { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool EsRelevante { get; set; }

    public DateTime FechaEntrega { get; set; }

    public string Estado { get; set; } = null!;

    public int? IdUsuarioAsignado { get; set; }

    public DateTime FechaCreacion { get; set; }
}
