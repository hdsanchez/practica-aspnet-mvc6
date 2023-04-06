using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Validations;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Models;

public class TipoCuentaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo Nombre es requerido")]
    [PrimeraLetraMayuscula]
    [Remote(action: "ValidacionTipoCuentaExistente", controller: "TiposCuentas")]
    public string Nombre { get; set; }

    public int UsuarioId { get; set; }

    public int Orden { get; set; }
}
