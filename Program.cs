using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            //Console.WriteLine(ex.Message);
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

        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            string sql = "DELETE FROM habit WHERE id = @id";
            SqliteCommand command2 = new SqliteCommand(sql, connection);
            command2.Parameters.AddWithValue("@id", id);

            command2.ExecuteNonQuery();

            Console.WriteLine("Deleted habit entry from table");

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return;
    }

    public static void updateDB(int id, int quantity, DateOnly date)
    {
        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            string sql = "UPDATE habit SET date = @date, quantity = @quantity WHERE id = @id";
            SqliteCommand command2 = new SqliteCommand(sql, connection);
            command2.Parameters.AddWithValue("@date", date);
            command2.Parameters.AddWithValue("@quantity", quantity);
            command2.Parameters.AddWithValue("@id", id);


            command2.ExecuteNonQuery();

            Console.WriteLine("Updated habit entry to table");

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }



        return;
    }

    public static void printRecords()
    {
        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            string sql2 = "INSERT INTO habit (date, quantity) VALUES (@date, @quantity)";
            SqliteCommand command2 = new SqliteCommand(sql2, connection);


            command2.ExecuteNonQuery();

            Console.WriteLine("Added habit entry to table");

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return;
    }


}

class Program
{

    static void Main(string[] args)
    {
        Database.createDB();

        bool quit = false;
        
        while(!quit)
        {
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

            string res = Console.ReadLine();

            res ??= "";

            switch (res)
            {
                case "0":
                    quit = true;
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
                    Console.Write("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    break;

            }
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

            if (dateStr.Equals("0"))
            {
                Console.Clear();
                return;
            }

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

            if (quantStr.Equals("0"))
            {
                Console.Clear();
                return;
            }

            if (int.TryParse(quantStr, out quantity) && quantity >= 0)
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

        Console.WriteLine();
        Console.Write("Press any key to continue");

        Console.ReadKey();

        Console.Clear();

        return;
    }

    static void Delete()
    {

        Console.WriteLine("Please enter the id of the entry you'd like to delete. Type 0 to return to the Main Menu.");

        bool passed = false;

        while (!passed)
        {
            string idStr = Console.ReadLine();

            idStr ??= "";

            if (idStr.Equals("0"))
            {
                Console.Clear();
                return;
            }

            if (int.TryParse(idStr, out int id))
            {
                Database.deleteDB(id);
                passed = true;
            }
            else
            {
                // Conversion failed, handle the error.
                Console.WriteLine("Invalid id. Enter a valid id. Type 0 to return to the Main Menu.");
            }
        }

        Console.WriteLine();
        Console.Write("Press any key to continue");

        Console.ReadKey();

        Console.Clear();

        return;
    }

    static void Update()
    {
        bool passed = false;
        int id = 0;
        int quantity = 0;
        DateOnly date = new DateOnly();

        Console.WriteLine("Enter the id of the entry to update");

        while (!passed)
        {
            string idStr = Console.ReadLine();

            idStr ??= "";

            if (idStr.Equals("0"))
            {
                Console.Clear();
                return;
            }

            if (int.TryParse(idStr, out id))
            {
                passed = true;
            }
            else
            {
                // Conversion failed, handle the error.
                Console.WriteLine("Invalid id. Enter a valid id. Type 0 to return to the Main Menu.");
            }
        }

        passed = false;

        Console.WriteLine("Please enter the date: (Format: dd-mm-yyyy). Type 0 to return to the Main Menu.");

        while (!passed)
        {
            string dateStr = Console.ReadLine();
            dateStr ??= "";

            if (dateStr.Equals("0"))
            {
                Console.Clear();
                return;
            }

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

            if (quantStr.Equals("0"))
            {
                Console.Clear();
                return;
            }

            if (int.TryParse(quantStr, out quantity) && quantity >= 0)
            {
                passed = true;

            }
            else
            {
                // Conversion failed, handle the error.
                Console.WriteLine("Invalid quantity");
            }
        }

        Database.updateDB(id, quantity, date);

        Console.WriteLine();
        Console.Write("Press any key to continue");

        Console.ReadKey();

        Console.Clear();

        return;
    }

    static void Records()
    {

        return;
    }
}

class Habit 
{
    private int id; 
    private int quantity; 
    private DateOnly date;

    public int Id { get => id; set => id = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public DateOnly Date { get => date; set => date = value; }
}
