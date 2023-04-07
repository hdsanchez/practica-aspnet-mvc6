using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers;

public class TiposCuentasController : Controller
{
	private readonly IRepositorioTiposCuentas _repositorioTiposCuentas;
	private readonly IServicioUsuarios _servicioUsuarios;

	public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios)
	{
		_repositorioTiposCuentas = repositorioTiposCuentas;
		_servicioUsuarios = servicioUsuarios;
	}

	public async Task<IActionResult> Index()
	{
		var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
		var tiposCuentas = await _repositorioTiposCuentas.Obtener(usuarioId);
		return View(tiposCuentas);
	}

	[HttpGet]
	public IActionResult Crear()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Crear(TipoCuentaViewModel tipoCuenta)
	{
		if (!ModelState.IsValid)
			return View(tipoCuenta);

		tipoCuenta.UsuarioId = _servicioUsuarios.ObtenerUsuarioId();

		var tipoCuentaExistente = await _repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
		if (tipoCuentaExistente)
		{
			ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
			return View(tipoCuenta);
		}

		await _repositorioTiposCuentas.Crear(tipoCuenta);

		return RedirectToAction("Index");
	}

	[HttpGet]
	public async Task<IActionResult> Editar(int id)
	{
		var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
		var tipoCuentas = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

		if (tipoCuentas is null) // El usuario en cuestión no tiene permisos para cargar el registro
			return RedirectToAction("NoEncontrado", "Home");

		return View(tipoCuentas);
	}

	[HttpPost]
	public async Task<IActionResult> Editar(TipoCuentaViewModel tipoCuenta)
	{
		var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
		var tipoCuentaExiste = await _repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);

		if (tipoCuentaExiste is null)
			return RedirectToAction("NoEncontrado", "Home");

		await _repositorioTiposCuentas.Actualizar(tipoCuenta);
		return RedirectToAction("Index");
	}

	public async Task<IActionResult> Borrar(int id)
	{
		var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
		var tipoCuenta = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);
		if (tipoCuenta is null)
			return RedirectToAction("NoEncontrado", "Home");
		return View(tipoCuenta);
	}

	[HttpPost]
	public async Task<IActionResult> BorrarTipoCuenta(int id)
	{
		var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
		var tipoCuenta = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);
		if (tipoCuenta is null)
			return RedirectToAction("NoEncontrado", "Home");
		await _repositorioTiposCuentas.Borrar(id);
		return RedirectToAction("Index");
	}

	[HttpGet]
	public async Task<IActionResult> ValidacionTipoCuentaExistente(string nombre)
	{
		var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
		var tipoCuentaExistente = await _repositorioTiposCuentas.Existe(nombre, usuarioId);
		return tipoCuentaExistente ? Json($"El nombre {nombre} ya existe.") : Json(true);
	}
}
