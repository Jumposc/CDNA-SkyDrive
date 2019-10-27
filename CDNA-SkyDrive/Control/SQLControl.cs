using MySql.Data.MySqlClient;
using System.Data;

namespace CDNA_SkyDrive.Control
{
    public class SQLControl
    {
        public static MySqlConnection connection = new MySqlConnection(Resources.GetResources("ConnectionString"));

        public static DataTable Select(string CommandText)
        {
            DataTable table = null;
            MySqlDataAdapter adapter = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                adapter = new MySqlDataAdapter(CommandText, connection);
                DataSet data = new DataSet();
                adapter.Fill(data, "Data1");
                table = data.Tables["Data1"];
                connection.Close();
            }
            catch (MySqlException e)
            {
                if (connection != null)
                    connection.Close();
            }
            return table;
        }

        public static int Select(string CommandText, params MySqlParameter[] parameter)
        {
            int i = -1;
            MySqlCommand command = null;
            MySqlDataReader dataReader = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                command = new MySqlCommand(CommandText, connection);
                if (parameter != null)
                    foreach (var item in parameter)
                    {
                        command.Parameters.Add(item);
                    }
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                    i = (int)dataReader[0];
                dataReader.Close();
                connection.Close();
            }
            catch
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
                if (connection != null)
                    connection.Close();
            }
            return i;
        }

        //public static MySqlDataReader Select(string CommandText, params MySqlParameter[] parameter)
        //{
        //    MySqlConnection connection = new MySqlConnection(Resources.GetResources("ConnectionString"));
        //    connection.Open();
        //    MySqlCommand command = new MySqlCommand(CommandText, connection);
        //    if (parameter != null)
        //        foreach (var item in parameter)
        //        {
        //            command.Parameters.Add(item);
        //        }
        //    MySqlDataReader read = command.ExecuteReader();
        //    connection.Close();
        //    return read;
        //}

        public static int Execute(string CommandText, params MySqlParameter[] parameter)
        {
            int i = 0;
            MySqlCommand command = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                command = new MySqlCommand(CommandText, connection);
                if (parameter != null)
                    foreach (var item in parameter)
                    {
                        command.Parameters.Add(item);
                    }
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                if (connection != null)
                    connection.Close();
            }
            return i;
        }
    }
}