using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services;

public interface IRepositorioCuentas
{
    Task<IEnumerable<CuentaViewModel>> Buscar(int usuarioId);
    Task Crear(CuentaViewModel cuenta);
}
