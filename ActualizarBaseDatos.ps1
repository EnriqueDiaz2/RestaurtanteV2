# Script para actualizar la base de datos con las nuevas columnas de configuración de impresora

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ACTUALIZADOR DE BASE DE DATOS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Buscar la base de datos
$dbPaths = @(
    "restaurante\bin\Debug\net6.0-windows\restaurante.db",
    "VersionPortable\restaurante.db"
)

$dbPath = $null
foreach ($path in $dbPaths) {
    if (Test-Path $path) {
        $dbPath = $path
        break
    }
}

if ($null -eq $dbPath) {
    Write-Host "? No se encontró la base de datos restaurante.db" -ForegroundColor Red
    Write-Host "   Ubicaciones buscadas:" -ForegroundColor Yellow
    foreach ($path in $dbPaths) {
        Write-Host "   - $path" -ForegroundColor Yellow
    }
    pause
    exit
}

Write-Host "? Base de datos encontrada: $dbPath" -ForegroundColor Green
Write-Host ""

# Cargar el ensamblado de SQLite
Add-Type -Path "restaurante\bin\Debug\net6.0-windows\Microsoft.Data.Sqlite.dll"

try {
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
    Write-Host "Ahora la aplicación puede recordar la impresora seleccionada" -ForegroundColor White
    Write-Host ""
    
} catch {
    Write-Host ""
    Write-Host "? Error al actualizar la base de datos:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
}

pause
