using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Supabase;
using Supabase.Postgrest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	}
}
