using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models;

public class CreadorCuentaViewModel : CuentaViewModel
{
    public IEnumerable<SelectListItem> TiposCuentas { get; set; }
}
