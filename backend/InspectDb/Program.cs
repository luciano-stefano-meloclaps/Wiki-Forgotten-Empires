using System;
using Microsoft.Data.Sqlite;

var dbPath = "../../database/wiki-forgotten-empires.db";
using var connection = new SqliteConnection($"Data Source={dbPath}");
connection.Open();

using var cmd = connection.CreateCommand();
cmd.CommandText = "SELECT id, name FROM Civilizations ORDER BY id";
using var reader = cmd.ExecuteReader();
int count = 0;
while (reader.Read())
{
    count++;
    Console.WriteLine($"#{count}: id={reader.GetInt32(0)}, name={reader.GetString(1)}");
}
Console.WriteLine($"TOTAL={count}");
