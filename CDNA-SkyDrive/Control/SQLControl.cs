using MySql.Data.MySqlClient;
using System.Data;

namespace CDNA_SkyDrive.Control
{
    public class SQLControl
    {
        public static DataTable Select(string CommandText)
        {
            MySqlConnection connection = new MySqlConnection(Resources.GetResources("ConnectionString"));
            connection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter(CommandText, connection);
            DataSet data = new DataSet();
            adapter.Fill(data, "Data1");
            connection.Close();
            return data.Tables["Data1"];
        }
        public static bool Select(string CommandText, params MySqlParameter[] parameter)
        {
            MySqlConnection connection = new MySqlConnection(Resources.GetResources("ConnectionString"));
            connection.Open();
            MySqlCommand command = new MySqlCommand(CommandText, connection);
            if (parameter != null)
                foreach (var item in parameter)
                {
                    command.Parameters.Add(item);
                }
            MySqlDataReader dataReader = command.ExecuteReader();
            bool k = dataReader.Read();
            dataReader.Close();
            connection.Close();
            return k;
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

        public static int Insert(string CommandText, params MySqlParameter[] parameter)
        {
            MySqlConnection connection = new MySqlConnection(Resources.GetResources("ConnectionString"));
            connection.Open();
            MySqlCommand command = new MySqlCommand(CommandText, connection);
            if (parameter != null)
                foreach (var item in parameter)
                {
                    command.Parameters.Add(item);
                }
            int i = command.ExecuteNonQuery();
            connection.Close();
            return i;
        }
    }
}