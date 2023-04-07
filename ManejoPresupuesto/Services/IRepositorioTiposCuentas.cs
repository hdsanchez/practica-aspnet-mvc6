using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services;

public interface IRepositorioTiposCuentas
{
	Task Actualizar(TipoCuentaViewModel tipoCuenta);
	Task Borrar(int id);
	Task Crear(TipoCuentaViewModel tipoCuenta);
    Task<bool> Existe(string nombre, int usuarioId);
    Task<IEnumerable<TipoCuentaViewModel>> Obtener(int usuarioId);
	Task<TipoCuentaViewModel> ObtenerPorId(int id, int usuarioId);
}
