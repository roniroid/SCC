using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCC_DATA.Settings;

namespace SCC_DATA
{
    public class DBDriver : IDisposable
	{
		public enum CONNECTION_STRINGS
		{
			DEFAULT_CONNECTION
		}

		public string GetConnectionString(CONNECTION_STRINGS connectionString = CONNECTION_STRINGS.DEFAULT_CONNECTION)
		{
			switch (connectionString)
			{
				default:
					return Constants.DEFAULT_CONNECTION;
			}
		}

		void FillWithParameters(SqlCommand command, SqlParameter[] parameters)
		{
			if (parameters != null)
			{
				parameters
					.ToList()
					.ForEach(p => command.Parameters.Add(p));
			}
		}

		public DataTable Select(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.StoredProcedure, CONNECTION_STRINGS connectionString = CONNECTION_STRINGS.DEFAULT_CONNECTION)
		{
			using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionString)))
			{
				try
				{
					SqlCommand command = new SqlCommand(commandText, (SqlConnection)connection);
					command.CommandType = commandType;

					FillWithParameters(command, parameters);

					connection.Open();

					SqlDataReader reader = command.ExecuteReader();

					DataTable dt = new DataTable();

					if (reader.HasRows)
					{
						dt.Load(reader);
					}

					return dt;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
				}
			}
		}

		public object ReadFirstColumn(SqlCommand command)
		{
			try
			{
				command.Connection.Open();

				object result = command.ExecuteScalar();

				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				command.Connection.Close();
			}
		}

		public object ReadFirstColumn(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.StoredProcedure, CONNECTION_STRINGS connectionString = CONNECTION_STRINGS.DEFAULT_CONNECTION)
		{
			using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionString)))
			{
				try
				{
					SqlCommand command = new SqlCommand(commandText, (SqlConnection)connection);
					command.CommandType = commandType;

					FillWithParameters(command, parameters);

					object result = ReadFirstColumn(command);

					return result;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
				}
			}
		}

		public int Execute(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.StoredProcedure, CONNECTION_STRINGS connectionString = CONNECTION_STRINGS.DEFAULT_CONNECTION)
		{
			using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionString)))
			{
				try
				{
					SqlCommand command = new SqlCommand(commandText, (SqlConnection)connection);
					command.CommandType = commandType;

					FillWithParameters(command, parameters);

					connection.Open();

					int result = command.ExecuteNonQuery();

					foreach (SqlParameter p in command.Parameters)
					{
						if (p.Direction == ParameterDirection.Output)
						{
							result = Int32.Parse(p.Value.ToString());
							break;
						}
					}

					return result;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
				}
			}
		}

		public SqlParameter CreateParameter(string name, object value, SqlDbType sqlDbType, ParameterDirection parameterDirection = ParameterDirection.Input, string typeName = null)
		{
			return new SqlParameter(name, sqlDbType)
			{
				Value = value,
				Direction = parameterDirection,
				TypeName = !string.IsNullOrEmpty(typeName) ? typeName : null
			};
		}

		public void Dispose()
		{

		}
	}
}
