using Microsoft.Data.Sqlite;
using System;
using System.IO;

// Script simple para actualizar la base de datos
Console.WriteLine("========================================");
Console.WriteLine("  ACTUALIZADOR DE BASE DE DATOS");
Console.WriteLine("========================================");
Console.WriteLine();

string[] dbPaths = new[]
{
    Path.Combine("restaurante", "bin", "Debug", "net6.0-windows", "restaurante.db"),
    Path.Combine("VersionPortable", "restaurante.db")
};

string? dbPath = null;
foreach (var path in dbPaths)
{
    if (File.Exists(path))
    {
        dbPath = path;
        break;
    }
}

if (dbPath == null)
{
    Console.WriteLine("? No se encontró la base de datos restaurante.db");
    Console.WriteLine("   Ubicaciones buscadas:");
    foreach (var path in dbPaths)
    {
        Console.WriteLine($"   - {path}");
    }
    Console.WriteLine("\nPresione Enter para salir...");
    Console.ReadLine();
    return;
}

Console.WriteLine($"? Base de datos encontrada: {dbPath}");
Console.WriteLine();

try
{
    var connectionString = $"Data Source={dbPath}";
    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    
    Console.WriteLine("? Verificando estructura actual...");
    
    // Verificar si las columnas ya existen
    var cmdCheck = connection.CreateCommand();
    cmdCheck.CommandText = "PRAGMA table_info(Configuraciones)";
    
    bool tieneImpresoraSeleccionada = false;
    bool tieneImprimirDirectamente = false;
    
    using (var reader = cmdCheck.ExecuteReader())
    {
        while (reader.Read())
        {
            string columnName = reader.GetString(1);
            if (columnName == "ImpresoraSeleccionada")
                tieneImpresoraSeleccionada = true;
            if (columnName == "ImprimirDirectamente")
                tieneImprimirDirectamente = true;
        }
    }
    
    // Agregar columnas si no existen
    if (!tieneImpresoraSeleccionada)
    {
        Console.WriteLine("? Agregando columna 'ImpresoraSeleccionada'...");
        var cmdAdd1 = connection.CreateCommand();
        cmdAdd1.CommandText = "ALTER TABLE Configuraciones ADD COLUMN ImpresoraSeleccionada TEXT NULL";
        cmdAdd1.ExecuteNonQuery();
        Console.WriteLine("? Columna 'ImpresoraSeleccionada' agregada");
    }
    else
    {
        Console.WriteLine("? La columna 'ImpresoraSeleccionada' ya existe");
    }
    
    if (!tieneImprimirDirectamente)
    {
        Console.WriteLine("? Agregando columna 'ImprimirDirectamente'...");
        var cmdAdd2 = connection.CreateCommand();
        cmdAdd2.CommandText = "ALTER TABLE Configuraciones ADD COLUMN ImprimirDirectamente INTEGER NOT NULL DEFAULT 0";
        cmdAdd2.ExecuteNonQuery();
        Console.WriteLine("? Columna 'ImprimirDirectamente' agregada");
    }
    else
    {
        Console.WriteLine("? La columna 'ImprimirDirectamente' ya existe");
    }
    
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("  ? BASE DE DATOS ACTUALIZADA");
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("Ahora la aplicación puede recordar la impresora seleccionada");
    Console.WriteLine();
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine("? Error al actualizar la base de datos:");
    Console.WriteLine(ex.Message);
    Console.WriteLine();
}

Console.WriteLine("Presione Enter para salir...");
Console.ReadLine();
