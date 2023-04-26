using ManejoPresupuesto.Services;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers;

public class CuentasController : Controller
{
    private readonly IRepositorioTiposCuentas _repoTiposCuentas;
    private readonly IServicioUsuarios _servicioUsuarios;

    public CuentasController(IRepositorioTiposCuentas repoTiposCuentas, IServicioUsuarios servicioUsuarios)
    {
        _repoTiposCuentas = repoTiposCuentas;
        _servicioUsuarios = servicioUsuarios;
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
        var tiposCuentas = await _repoTiposCuentas.Obtener(usuarioId);
        var modelo = new CreadorCuentaViewModel();
        modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        return View(modelo);
    }
}
