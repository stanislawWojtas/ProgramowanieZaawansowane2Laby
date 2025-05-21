using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace MvpLogin.Models;

public static class DatabaseHelper
{
	public static bool CreateTables(SqliteConnection connection)
	{
		try
		{
			SqliteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "DROP TABLE IF EXISTS Logins";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "DROP TABLE IF EXISTS Products";
			cmd.ExecuteNonQuery();

			cmd.CommandText = @"
					CREATE TABLE Logins (
						Id INTEGER PRIMARY KEY AUTOINCREMENT,
						Username TEXT NOT NULL,
						Password TEXT NOT NULL
					);";
			cmd.ExecuteNonQuery();

			cmd.CommandText = @"
					CREATE TABLE Products (
						Id INTEGER PRIMARY KEY AUTOINCREMENT,
						Title TEXT NOT NULL,
						Author TEXT NOT NULL,
						Year TEXT
					);";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "SELECT COUNT(*) FROM Logins";
			long count = (long)cmd.ExecuteScalar();
			if (count == 0)
			{
				//if there is no user we have to add admin
				AddUser("admin", "1234");
			}


		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return false;
		}
		return true;
	}

	public static List<Product> GetAllProducts()
	{
		var products = new List<Product>();
		var connectionStringBuilder = new SqliteConnectionStringBuilder();
		connectionStringBuilder.DataSource = "./Data/data.db";

		using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "SELECT Title, Author, Year FROM Products";
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var product = new Product
						{
							Title = reader.GetString(0),
							Author = reader.GetString(1),
							Year = reader.GetString(2)
						};
						products.Add(product);
					}
				}
			}
		}
		return products;
	}


	public static void AddProduct(Product product)
	{
		var connectionStringBuilder = new SqliteConnectionStringBuilder
		{
			DataSource = "./Data/data.db"
		};

		using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
		connection.Open();
		using var command = connection.CreateCommand();
		command.CommandText = "INSERT INTO Products (Title, Author, Year) VALUES (@title, @author, @year)";
		command.Parameters.AddWithValue("@title", product.Title);
		command.Parameters.AddWithValue("@author", product.Author);
		command.Parameters.AddWithValue("@year", product.Year);
		command.ExecuteNonQuery();
	}

	public static bool AddUser(string username, string password)
	{
		var connectionStringBuilder = new SqliteConnectionStringBuilder
		{
			DataSource = "./Data/data.db"
		};

		using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
		connection.Open();

		using (var checkCmd = connection.CreateCommand())
		{
			checkCmd.CommandText = "SELECT COUNT(*) FROM Logins WHERE Username = @username";
			checkCmd.Parameters.AddWithValue("@username", username);
			long exist = (long)checkCmd.ExecuteScalar();
			if (exist > 0)
			{
				//user already exist
				return false;
			}
		}

		using (var cmd = connection.CreateCommand())
		{
			cmd.CommandText = "INSERT INTO Logins (Username, Password) VALUES (@username, @password)";
			cmd.Parameters.AddWithValue("@username", username);
			cmd.Parameters.AddWithValue("@password", getMD5(password));
			cmd.ExecuteNonQuery();
		}
		return true;

	}


	public static string getMD5(string input)
	{
		using var md5 = MD5.Create();
		byte[] inputBytes = Encoding.ASCII.GetBytes(input);
		byte[] hashBytes = md5.ComputeHash(inputBytes);
		return Convert.ToHexString(hashBytes);
	}

	public static bool CheckLogin(string username, string password)
	{
		var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./Data/data.db" };
		using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
		connection.Open();
		using var cmd = connection.CreateCommand();

		cmd.CommandText = "SELECT Password FROM Logins WHERE Username = @username";
		cmd.Parameters.AddWithValue("@username", username);

		var result = cmd.ExecuteScalar();
		if (result == null)
			return false;
		string storedHash = result.ToString();
		string inputHash = getMD5(password);

		return storedHash == inputHash;
	}


	public static void Connect()
	{
		var connectionStringBuilder = new SqliteConnectionStringBuilder
		{
			DataSource = "./Data/data.db"
		};

		using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
		{
			connection.Open();
			CreateTables(connection);
		}
	}
}