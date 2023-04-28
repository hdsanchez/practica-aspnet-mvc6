using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services;

public interface IRepositorioCuentas
{
    Task Crear(CuentaViewModel cuenta);
    Task<IEnumerable<CuentaViewModel>> Buscar(int usuarioId);
    Task<CuentaViewModel> ObtenerPorId(int id, int usuarioId);
    Task Actualizar(CreadorCuentaViewModel cuenta);
}
