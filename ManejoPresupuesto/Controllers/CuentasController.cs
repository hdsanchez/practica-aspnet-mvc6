using ManejoPresupuesto.Services;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers;

public class CuentasController : Controller
{
    private readonly IRepositorioTiposCuentas _repoTiposCuentas;
    private readonly IServicioUsuarios _servicioUsuarios;
    private readonly IRepositorioCuentas _repoCuentas;

    public CuentasController(IRepositorioTiposCuentas repoTiposCuentas, IServicioUsuarios servicioUsuarios, IRepositorioCuentas repoCuentas)
    {
        _repoTiposCuentas = repoTiposCuentas;
        _servicioUsuarios = servicioUsuarios;
        _repoCuentas = repoCuentas;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
        var cuentasConTipoCuenta = await _repoCuentas.Buscar(usuarioId);
        var modelo = cuentasConTipoCuenta
            .GroupBy(x => x.TipoCuenta)
            .Select(grupo => new IndiceCuentasViewModel
            {
                TipoCuenta = grupo.Key,
                Cuentas = grupo.AsEnumerable()
            }).ToList();
        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
        var modelo = new CreadorCuentaViewModel();
        modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CreadorCuentaViewModel cuenta)
    {
        var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
        var tipoCuenta = await _repoTiposCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);
        if (tipoCuenta is null) return RedirectToAction("NoEncontrado", "Home");
        if (!ModelState.IsValid)
        {
            cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(cuenta);
        }
        await _repoCuentas.Crear(cuenta);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Editar(CreadorCuentaViewModel cuentaEditar)
    {
        var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
        var cuenta = await _repoCuentas.ObtenerPorId(cuentaEditar.Id, usuarioId);
        if (cuenta is null)
            return RedirectToAction("NoEncontrado", "Home");
        var tipoCuenta = await _repoTiposCuentas.ObtenerPorId(cuentaEditar.TipoCuentaId, usuarioId);
        if (tipoCuenta is null)
            return RedirectToAction("NoEncontrado", "Home");
        await _repoCuentas.Actualizar(cuentaEditar);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Editar(int id)
    {
        var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
        var cuenta = await _repoCuentas.ObtenerPorId(id, usuarioId);
        if (cuenta is null)
            return RedirectToAction("NoEncontrado", "Home");
        var modelo = new CreadorCuentaViewModel
        {
            Id = cuenta.Id,
            Nombre = cuenta.Nombre,
            TipoCuentaId = cuenta.TipoCuentaId,
            Descripcion = cuenta.Descripcion,
            Balance = cuenta.Balance
        };
        modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
        return View(modelo);
    }

    private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
    {
        var tiposCuentas = await _repoTiposCuentas.Obtener(usuarioId);
        return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
    }
}
