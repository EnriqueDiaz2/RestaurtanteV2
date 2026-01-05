using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ActualizarDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbPath = Path.Combine("bin", "Debug", "net6.0-windows", "restaurante.db");
            
            if (!File.Exists(dbPath))
            {
                Console.WriteLine($"? No se encontró la base de datos en: {dbPath}");
                Console.WriteLine("Intentando en VersionPortable...");
                dbPath = Path.Combine("..", "VersionPortable", "restaurante.db");
                
                if (!File.Exists(dbPath))
                {
                    Console.WriteLine("? No se encontró la base de datos.");
                    return;
                }
            }
            
            try
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    
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
                        var cmdAdd1 = connection.CreateCommand();
                        cmdAdd1.CommandText = "ALTER TABLE Configuraciones ADD COLUMN ImpresoraSeleccionada TEXT NULL";
                        cmdAdd1.ExecuteNonQuery();
                        Console.WriteLine("? Columna 'ImpresoraSeleccionada' agregada");
                    }
                    else
                    {
                        Console.WriteLine("? Columna 'ImpresoraSeleccionada' ya existe");
                    }
                    
                    if (!tieneImprimirDirectamente)
                    {
                        var cmdAdd2 = connection.CreateCommand();
                        cmdAdd2.CommandText = "ALTER TABLE Configuraciones ADD COLUMN ImprimirDirectamente INTEGER NOT NULL DEFAULT 0";
                        cmdAdd2.ExecuteNonQuery();
                        Console.WriteLine("? Columna 'ImprimirDirectamente' agregada");
                    }
                    else
                    {
                        Console.WriteLine("? Columna 'ImprimirDirectamente' ya existe");
                    }
                    
                    Console.WriteLine("\n? Base de datos actualizada correctamente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n? Error al actualizar la base de datos: {ex.Message}");
            }
        }
    }
}
