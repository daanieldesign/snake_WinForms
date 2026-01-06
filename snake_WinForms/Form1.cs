using System.Data.SQLite;
using System.IO;

void InitDatabase()
{
    // Vytvoří soubor, pokud neexistuje
    if (!File.Exists(dbPath))
        SQLiteConnection.CreateFile(dbPath);

    using (var con = new SQLiteConnection($"Data Source={dbPath}"))
    {
        con.Open();

        // Vytvoří tabulku, pokud neexistuje
        string sql = @"CREATE TABLE IF NOT EXISTS Results (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Score INTEGER,
                        Lives INTEGER,
                        Date TEXT
                       )";
        new SQLiteCommand(sql, con).ExecuteNonQuery();

        // Zkontroluj, zda existuje sloupec Name, pokud ne, přidej ho
        try
        {
            string checkSql = "SELECT Name FROM Results LIMIT 1";
            new SQLiteCommand(checkSql, con).ExecuteScalar();
        }
        catch
        {
            // sloupec Name neexistuje → přidáme ho
            string alterSql = "ALTER TABLE Results ADD COLUMN Name TEXT";
            new SQLiteCommand(alterSql, con).ExecuteNonQuery();
        }
    }
}
