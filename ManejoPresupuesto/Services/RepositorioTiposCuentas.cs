using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services;

public class RepositorioTiposCuentas : IRepositorioTiposCuentas
{
	private readonly string _connectionString;

	public RepositorioTiposCuentas(IConfiguration configuration)
	{
		_connectionString = configuration.GetConnectionString("DefaultConnection");
	}

	public async Task Crear(TipoCuentaViewModel tipoCuenta)
	{
		using var connection = new SqlConnection(_connectionString);
		var id = await connection.QuerySingleAsync<int>(
			@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden) 
			VALUES (@Nombre, @UsuarioId, 0); 
			SELECT SCOPE_IDENTITY();",
			tipoCuenta);
		tipoCuenta.Id = id;
	}

	public async Task<bool> Existe(string nombre, int usuarioId)
	{
		using var connection = new SqlConnection(_connectionString);
		var existe = await connection.QueryFirstOrDefaultAsync<int>(
			@"SELECT 1 FROM TiposCuentas 
            WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
			new { nombre, usuarioId });
		return existe == 1;
	}

	public async Task<IEnumerable<TipoCuentaViewModel>> Obtener(int usuarioId)
	{
		using var connection = new SqlConnection(_connectionString);
		return await connection.QueryAsync<TipoCuentaViewModel>(
			@"SELECT Id, Nombre, Orden " +
			"FROM TiposCuentas " +
			"WHERE UsuarioId = @UsuarioId;",
			new { usuarioId });
	}

	public async Task Actualizar(TipoCuentaViewModel tipoCuenta)
	{
		using var connection = new SqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"UPDATE TiposCuentas 
            SET Nombre = @Nombre 
            WHERE Id = @Id",
			tipoCuenta);
	}

	public async Task<TipoCuentaViewModel> ObtenerPorId(int id, int usuarioId)
	{
		using var connection = new SqlConnection(_connectionString);
		return await connection.QueryFirstOrDefaultAsync<TipoCuentaViewModel>(
			@"SELECT Id, Nombre, Orden
            FROM TiposCuentas
            WHERE Id = @Id AND UsuarioId = @UsuarioId",
			new { id, usuarioId });
	}

	public async Task Borrar(int id)
	{
		using var connection = new SqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"DELETE TiposCuentas
			WHERE Id = @Id",
			new { id });
	}
}
