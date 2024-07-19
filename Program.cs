using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using static System.Formats.Asn1.AsnWriter;

class Database
{

    public static void createDB()
    {
        var sql = @"CREATE TABLE habit(
                  id INTEGER PRIMARY KEY,
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

    public static void insertDB(int quantity, DateOnly date)
    {
        
        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            string sql2 = "INSERT INTO habit (date, quantity) VALUES (@date, @quantity)";
            SqliteCommand command2 = new SqliteCommand(sql2, connection);
            command2.Parameters.AddWithValue("@date", date);
            command2.Parameters.AddWithValue("@quantity", quantity);

            command2.ExecuteNonQuery();

            Console.WriteLine("Added habit entry to table");

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public static void deleteDB(int id)
    {

        return;
    }


}

class Program
{

    static void Main(string[] args)
    {
        Database.createDB();

        Console.WriteLine("MAIN MENU");
        Console.WriteLine();
        Console.WriteLine("What would you like to do?");
        Console.WriteLine();
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View All Records");
        Console.WriteLine("Type 2 to Insert Record");
        Console.WriteLine("Type 3 to Delete Record");
        Console.WriteLine("Type 4 to Update Record");
        Console.WriteLine("-----------------------------------------");

        String res = Console.ReadLine();

        res ??= "";

        switch (res)
        {
            case "0":
                return;
            case "1":
                Records();
                break;
            case "2":
                Insert();
                break;
            case "3":
                Delete();
                break;
            case "4":
                Update();
                break;
            default:
                Console.WriteLine("Invalid Input");
                Console.WriteLine("");
                Console.WriteLine("");
                break;

        }

    }

    static void Insert()
    {
        Console.WriteLine("Please enter the date: (Format: dd-mm-yyyy). Type 0 to return to the Main Menu.");

        bool passed = false;
        int quantity = 0;
        DateOnly date = new DateOnly();

        while (!passed)
        {
            string dateStr = Console.ReadLine();
            dateStr ??= "";

            try
            {
                string dateFormat = "dd-MM-yyyy";
                date = DateOnly.ParseExact(dateStr, dateFormat);

                passed = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid date. (Format: dd-mm-yyyy)");
            }
        }

        passed = false;

        Console.WriteLine("Please enter the cups of water drank. Type 0 to return to the Main Menu.");


        while (!passed)
        {
            string quantStr = Console.ReadLine();

            quantStr ??= "";

            if (int.TryParse(quantStr, out quantity) && quantity > 0)
            {
                passed = true;
            }
            else
            {
                // Conversion failed, handle the error.
                Console.WriteLine("Invalid quantity");
            }
        }

        Database.insertDB(quantity, date);

        return;
    }

    static void Delete()
    {

        return;
    }

    static void Update()
    {

        return;
    }

    static void Records()
    {

        return;
    }
}

class Converesions
{
    static DateOnly? StringToDate(string dateStr)
    {


        return null;
    }
}