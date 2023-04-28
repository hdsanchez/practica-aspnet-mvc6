using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services;

public class RepositorioCuentas : IRepositorioCuentas
{
    private readonly string _connectionString;

    public RepositorioCuentas(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task Crear(CuentaViewModel cuenta)
    {
        using var connection = new SqlConnection(_connectionString);
        var id = await connection.QuerySingleAsync<int>(
            @"INSERT INTO Cuentas (Nombre, TipoCuentaId, Descripcion, Balance)
            VALUES (@Nombre, @TipoCuentaId, @Descripcion, @Balance);
            SELECT SCOPE_IDENTITY();",
            cuenta);
        cuenta.Id = id;
    }

    public async Task<IEnumerable<CuentaViewModel>> Buscar(int usuarioId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<CuentaViewModel>(
            @"SELECT Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre AS TipoCuenta
            FROM Cuentas
            INNER JOIN TiposCuentas tc
            ON tc.Id = Cuentas.TipoCuentaId
            WHERE tc.UsuarioId = @UsuarioId
            ORDER BY tc.Orden",
            new { usuarioId });
    }

    public async Task<CuentaViewModel> ObtenerPorId(int id, int usuarioId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<CuentaViewModel>(
            @"SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, tc.Id
            FROM Cuentas
            INNER JOIN TiposCuentas tc
            ON tc.Id = Cuentas.TipoCuentaId
            WHERE tc.UsuarioId = @UsuarioId AND Cuentas.Id = @Id",
            new {id, usuarioId});
    }

    public async Task Actualizar(CreadorCuentaViewModel cuenta)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            @"UPDATE Cuentas
            SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion, TipoCuentaId = @TipoCuentaId
            WHERE Id = @Id;",
            cuenta);
    }
}
