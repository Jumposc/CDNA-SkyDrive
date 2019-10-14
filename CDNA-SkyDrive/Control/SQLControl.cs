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

        public static int Insert(string CommandText)
        {
            MySqlConnection connection = new MySqlConnection(Resources.GetResources("ConnectionString"));
            connection.Open();
            MySqlCommand command = new MySqlCommand(CommandText, connection);
            int i = command.ExecuteNonQuery();
            connection.Close();
            return i;
        }
    }
}