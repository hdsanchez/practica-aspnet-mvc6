namespace ManejoPresupuesto.Models;

public class IndiceCuentasViewModel
{
    public string TipoCuenta { get; set; }

    public IEnumerable<CuentaViewModel> Cuentas { get; set; }

    public decimal Balance => Cuentas.Sum(x => x.Balance);
}
