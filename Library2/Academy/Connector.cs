using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Academy
{
	static internal class Connector
	{
		//статическим называется класс, который содержит только статические поля и методы

		static string connectionString = "";
		static SqlConnection connection = null;
		static Connector() 
		{
			connectionString = ConfigurationManager.ConnectionStrings["Academy"].ConnectionString;
			connection = new SqlConnection(connectionString);
		}

		public static DataTable LoadData(string fields, string tables, string condition = "")
		{
			DataTable table = null;
			string cmd = $"SELECT {fields} FROM {tables}";
			if (condition.Length > 0) cmd += $" WHERE {condition}";
			cmd += ";";

			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			if(reader.HasRows)
			{
				table = new DataTable();
				for(int i=0;i<reader.FieldCount;i++)
					table.Columns.Add(reader.GetName(i));
				while (reader.Read())
				{
					DataRow row = table.NewRow();
					for(int i=0;i<reader.FieldCount;i++)
						row[i] = reader[i];
					table.Rows.Add(row);
				}
			}

			reader.Close();
			connection.Close();
			return table;
		}
		public static Dictionary<string, int> LoadPair(string field_name, string field_id, string tables, string condition = "")
		{
			Dictionary<string, int> dictionary = null;
			string cmd = $"SELECT {field_name}, {field_id} FROM {tables}";
			if (condition.Length > 0) cmd += $" WHERE {condition}";
			cmd += ";";

			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			if(reader.HasRows)
			{
				dictionary = new Dictionary<string, int>();
				while(reader.Read())
				{
					dictionary[reader[0].ToString()] = Convert.ToInt32(reader[1]);
				}
			}

			connection.Close();

			return dictionary;
		}
		
	}
}
