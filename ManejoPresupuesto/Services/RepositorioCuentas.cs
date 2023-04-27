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
}
