using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

class Database
{

    public static void createDB()
    {
        var sql = @"CREATE TABLE habit(
                  date DATE,
                  quantity INT
                  )";

        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();

            Console.WriteLine("Table 'habit' created successfully.");

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public static void insertDB(int quantity)
    {
        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            string sql2 = "INSERT INTO habit (date, quantity) VALUES (@date, @quantity)";
            SqliteCommand command2 = new SqliteCommand(sql2, connection);
            command2.Parameters.AddWithValue("@date", DateOnly.FromDateTime(DateTime.Now));
            command2.Parameters.AddWithValue("@quantity", quantity);

            command2.ExecuteNonQuery();

            Console.WriteLine("Added habit entry to table");

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

}

class Program
{

    static void Main(string[] args)
    {
        Database.createDB();

        Database.insertDB(7);
    }
}