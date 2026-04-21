using System;
using Microsoft.Data.Sqlite;
using System.IO;

class Program {
    static void Main() {
        var db = @"d:\Proyectos\Fullstack\ForgottensEmpiresFullstack\database\wiki-forgotten-empires.db";
        Console.WriteLine($"DB exists: {File.Exists(db)}");
        if (!File.Exists(db)) return;
        Console.WriteLine("Current dir: " + Directory.GetCurrentDirectory());
        using var conn = new SqliteConnection($"Data Source={db}");
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, Name FROM Civilizations WHERE Name LIKE '%AANotion%' COLLATE NOCASE LIMIT 10";
        using var reader = cmd.ExecuteReader();
        Console.WriteLine("AANotion rows:");
        while (reader.Read()) {
            Console.WriteLine($"{reader.GetInt32(0)}\t{reader.GetString(1)}");
        }
        reader.Close();
        cmd.CommandText = "SELECT id, Name FROM Civilizations LIMIT 20";
        using var reader2 = cmd.ExecuteReader();
        Console.WriteLine("--- sample rows ---");
        while (reader2.Read()) {
            Console.WriteLine($"{reader2.GetInt32(0)}\t{reader2.GetString(1)}");
        }
    }
}
