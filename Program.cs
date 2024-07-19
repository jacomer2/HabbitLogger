using Microsoft.Data.Sqlite;


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

            string sql = "INSERT INTO habit (date, quantity) VALUES (@date, @quantity)";
            SqliteCommand command2 = new SqliteCommand(sql, connection);
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

            if (getRecord(id) == null)
            {
                Console.WriteLine("User not found in db");
                Console.WriteLine();
                return;
            }

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

            if (getRecord(id) == null)
            {
                Console.WriteLine("User not found in db");
                Console.WriteLine();
                return;
            }

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

    public static List<Habit>? printRecords()
    {

        List<Habit> habitList = new List<Habit>();
        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            string sql = "SELECT * FROM habit";

            var command = new SqliteCommand(sql, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string dateStr = reader.GetString(1);
                int quantity = reader.GetInt32(2);

                string dateFormat = "yyyy-MM-dd";
                DateOnly date = DateOnly.ParseExact(dateStr, dateFormat);

                Habit habit = new Habit(id, date, quantity);

                //Console.WriteLine($"Id: {id}, Date: {dateStr}, Quantity: {quantity}");

                habitList.Add(habit);
            }


            Console.WriteLine("End of print records");

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return habitList;
    }

    public static Habit? getRecord(int id)
    {
        try
        {
            using var connection = new SqliteConnection(@"Data Source=C:\Users\jacom\Documents\LearnC#\HabbitLogger\db\pub.db");
            connection.Open();

            string sql = "SELECT * FROM habit WHERE id = @id";

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);


            var reader = command.ExecuteReader();

            if (reader.Read())
            {

                string dateStr = reader.GetString(1);
                int quantity = reader.GetInt32(2);

                string dateFormat = "yyyy-MM-dd";
                DateOnly date = DateOnly.ParseExact(dateStr, dateFormat);

                Habit habit = new Habit(id, date, quantity);

                return habit;
            }

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;

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
                    Console.Clear();
                    Records();
                    break;
                case "2":
                    Console.Clear();
                    Insert();
                    break;
                case "3":
                    Console.Clear();
                    Delete();
                    break;
                case "4":
                    Console.Clear();
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
        Console.WriteLine("Please enter the date: (Format: yyyy-mm-dd). Type 0 to return to the Main Menu.");

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
                string dateFormat = "yyyy-MM-dd";
                date = DateOnly.ParseExact(dateStr, dateFormat);

                passed = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid date. (Format: yyyy-mm-dd)");
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

        Console.WriteLine("Enter the id of the entry to update. Type 0 to return to the Main Menu.");

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

        Console.WriteLine("Please enter the date: (Format: yyyy-mm-dd). Type 0 to return to the Main Menu.");

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
                string dateFormat = "yyyy-MM-dd";
                date = DateOnly.ParseExact(dateStr, dateFormat);

                passed = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid date. (Format: yyyy-mm-dd)");
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
        List<Habit> habits = Database.printRecords();

        Console.WriteLine("-------------------------------------------------------");

        foreach (Habit habit in habits)
        {

            string dateStr = habit.Date.ToString("yyyy-MM-dd");
            string idStr = habit.Id.ToString();
            string quantStr = habit.Quantity.ToString();

            Console.WriteLine($"| Id: {idStr} | Date: {dateStr} | Quantity: {quantStr} |");
        }

        Console.WriteLine("-------------------------------------------------------");

        Console.WriteLine();
        Console.WriteLine("Press any key to return to the Main Menu");
        Console.ReadKey();
        Console.Clear();

        return;
    }
}

class Habit 
{
    private int id; 
    private int quantity; 
    private DateOnly date;

    public Habit(int id, DateOnly date, int quantity)
    {
        this.id = id;
        this.date = date;
        this.quantity = quantity;
    }

    public int Id { get => id; set => id = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public DateOnly Date { get => date; set => date = value; }
}
