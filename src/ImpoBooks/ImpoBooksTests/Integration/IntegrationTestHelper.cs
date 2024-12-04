using ImpoBooks.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Supabase;
using Supabase.Postgrest.Responses;

namespace ImpoBooks.Tests.Integration
{
	internal static class IntegrationTestHelper
	{
		public static Client TestClientInit()
		{
			var configuration = ConfigurationSetup();

			var options = new SupabaseOptions
			{
				AutoConnectRealtime = true
			};

			return new Client(configuration["TestSupabase:Url"], configuration["TestSupabase:Key"], options);
		}

		private static IConfiguration ConfigurationSetup()
		{
			string currentPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
			IConfiguration configuration = new ConfigurationBuilder()
				.SetBasePath(currentPath)
				.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
				.Build();
			return configuration;
		}

		public static async Task RecreateTable<T>(Client client, IEnumerable<T> preparedData) where T : BaseModelExtended, new()
		{
			ModeledResponse<T> response = await client.From<T>().Get();
			IEnumerable<T> allRecords = response.Models;

			foreach (T record in allRecords)
			{
				await client.From<T>().Delete(record);
			}

			foreach (T record in preparedData)
			{
				await client.From<T>().Insert(record);
			}
		}

		public static async Task InitTable<T>(Client client, IEnumerable<T> preparedData) where T : BaseModelExtended, new()
		{
			foreach (T record in preparedData)
			{
				await client.From<T>().Insert(record);
			}
		}

		public static async Task ClearTable<T>(Client client) where T : BaseModelExtended, new()
		{
			ModeledResponse<T> response = await client.From<T>().Get();
			IEnumerable<T> allRecords = response.Models;

			foreach (T record in allRecords)
			{
				await client.From<T>().Delete(record);
			}
		}

		public async static Task<NpgsqlConnection> ConnectToDb()
		{
			var configuration = ConfigurationSetup();

			var connection = new NpgsqlConnection(configuration["ConnectionStrings:Test"]);
			await connection.OpenAsync();

			return connection;
		}

		public static async Task ClearDb()
		{
			var connection = await ConnectToDb();

			var command = new NpgsqlCommand("TRUNCATE TABLE \"Persons\", \"Publishers\", \"Users\", \"Genres\" CASCADE", connection);
			command.CommandType = System.Data.CommandType.Text;

			await command.ExecuteNonQueryAsync();

			command.Dispose();
			await connection.CloseAsync();
		}


		public static async Task AddRecordToDb(string query)
		{
			using var connection = await ConnectToDb();

			using var command = new NpgsqlCommand(query, connection);
			command.CommandType = System.Data.CommandType.Text;
			await command.ExecuteNonQueryAsync();

			command.Dispose();
			await connection.CloseAsync();
		}

		public async static Task RefreshDb(string query)
		{
			await ClearDb();
			await AddRecordToDb(query);
		}
	}
}
