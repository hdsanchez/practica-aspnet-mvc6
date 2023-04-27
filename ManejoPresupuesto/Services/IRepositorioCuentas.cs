using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services;

public interface IRepositorioCuentas
{
    Task Crear(CuentaViewModel cuenta);
}
