# Script para actualizar la base de datos agregando las columnas de impresora
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ACTUALIZADOR DE BASE DE DATOS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Buscar las bases de datos
$dbPaths = @(
    "VersionPortable\restaurante.db",
    "restaurante\bin\Debug\net6.0-windows\restaurante.db"
)

foreach ($dbPath in $dbPaths) {
    if (Test-Path $dbPath) {
        Write-Host "Procesando: $dbPath" -ForegroundColor Yellow
        Write-Host ""
        
        # Crear backup
        $backupPath = "$dbPath.backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
        Copy-Item $dbPath $backupPath
        Write-Host "? Backup creado: $backupPath" -ForegroundColor Green
        
        # Cargar SQLite
        Add-Type -Path "restaurante\bin\Debug\net6.0-windows\Microsoft.Data.Sqlite.dll"
        Add-Type -Path "restaurante\bin\Debug\net6.0-windows\SQLitePCLRaw.core.dll"
        Add-Type -Path "restaurante\bin\Debug\net6.0-windows\SQLitePCLRaw.provider.e_sqlite3.dll"
        Add-Type -Path "restaurante\bin\Debug\net6.0-windows\SQLitePCLRaw.batteries_v2.dll"
        
        try {
            # Inicializar SQLite
            [SQLitePCL.Batteries_V2]::Init()
            
            $connectionString = "Data Source=$dbPath"
            $connection = New-Object Microsoft.Data.Sqlite.SqliteConnection($connectionString)
            $connection.Open()
            
            Write-Host "? Verificando estructura actual..." -ForegroundColor Yellow
            
            # Verificar si las columnas ya existen
            $cmdCheck = $connection.CreateCommand()
            $cmdCheck.CommandText = "PRAGMA table_info(Configuraciones)"
            $reader = $cmdCheck.ExecuteReader()
            
            $tieneImpresoraSeleccionada = $false
            $tieneImprimirDirectamente = $false
            
            while ($reader.Read()) {
                $columnName = $reader.GetString(1)
                if ($columnName -eq "ImpresoraSeleccionada") {
                    $tieneImpresoraSeleccionada = $true
                }
                if ($columnName -eq "ImprimirDirectamente") {
                    $tieneImprimirDirectamente = $true
                }
            }
            $reader.Close()
            
            # Agregar columnas si no existen
            if (-not $tieneImpresoraSeleccionada) {
                Write-Host "? Agregando columna 'ImpresoraSeleccionada'..." -ForegroundColor Yellow
                $cmdAdd1 = $connection.CreateCommand()
                $cmdAdd1.CommandText = "ALTER TABLE Configuraciones ADD COLUMN ImpresoraSeleccionada TEXT NULL"
                $cmdAdd1.ExecuteNonQuery() | Out-Null
                Write-Host "? Columna 'ImpresoraSeleccionada' agregada" -ForegroundColor Green
            } else {
                Write-Host "? La columna 'ImpresoraSeleccionada' ya existe" -ForegroundColor Cyan
            }
            
            if (-not $tieneImprimirDirectamente) {
                Write-Host "? Agregando columna 'ImprimirDirectamente'..." -ForegroundColor Yellow
                $cmdAdd2 = $connection.CreateCommand()
                $cmdAdd2.CommandText = "ALTER TABLE Configuraciones ADD COLUMN ImprimirDirectamente INTEGER NOT NULL DEFAULT 0"
                $cmdAdd2.ExecuteNonQuery() | Out-Null
                Write-Host "? Columna 'ImprimirDirectamente' agregada" -ForegroundColor Green
            } else {
                Write-Host "? La columna 'ImprimirDirectamente' ya existe" -ForegroundColor Cyan
            }
            
            $connection.Close()
            
            Write-Host ""
            Write-Host "========================================" -ForegroundColor Green
            Write-Host "  ? BASE DE DATOS ACTUALIZADA" -ForegroundColor Green
            Write-Host "========================================" -ForegroundColor Green
            Write-Host ""
        }
        catch {
            Write-Host ""
            Write-Host "? Error al actualizar la base de datos:" -ForegroundColor Red
            Write-Host $_.Exception.Message -ForegroundColor Red
            Write-Host ""
            
            # Restaurar backup si hay error
            if (Test-Path $backupPath) {
                Copy-Item $backupPath $dbPath -Force
                Write-Host "? Base de datos restaurada desde backup" -ForegroundColor Yellow
            }
        }
    }
}

Write-Host "Presione Enter para salir..."
Read-Host
