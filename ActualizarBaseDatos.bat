@echo off
echo ========================================
echo    ACTUALIZADOR DE BASE DE DATOS
echo    Sistema de Restaurante POS
echo ========================================
echo.
echo Este script creará las tablas necesarias
echo para el sistema de ventas cerradas.
echo.

REM Buscar el archivo restaurante.db
set DB_PATH=restaurante\bin\Debug\net6.0-windows\restaurante.db
if exist "%DB_PATH%" (
    echo Encontrada base de datos en: %DB_PATH%
) else (
    set DB_PATH=restaurante\bin\Release\net6.0-windows\win-x64\restaurante.db
    if exist "%DB_PATH%" (
        echo Encontrada base de datos en: %DB_PATH%
    ) else (
        set DB_PATH=restaurante.db
        if exist "%DB_PATH%" (
            echo Encontrada base de datos en: %DB_PATH%
        ) else (
            echo ERROR: No se encontró el archivo restaurante.db
            echo.
            echo Por favor, ejecute este script desde la carpeta raíz del proyecto
            echo o asegúrese de que la base de datos existe.
            echo.
            pause
            exit /b 1
        )
    )
)

echo.
echo Verificando si sqlite3.exe está disponible...
where sqlite3 >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ADVERTENCIA: sqlite3.exe no está en el PATH del sistema
    echo.
    echo Por favor, siga estas instrucciones manuales:
    echo.
    echo 1. Descargue DB Browser for SQLite desde: https://sqlitebrowser.org/
    echo 2. Instale el programa
    echo 3. Abra el archivo: %DB_PATH%
    echo 4. Vaya a la pestaña "Ejecutar SQL"
    echo 5. Copie y pegue el contenido del archivo CrearTablasVentas.sql
    echo 6. Haga clic en "Ejecutar"
    echo 7. Guarde los cambios
    echo.
    echo O siga estos pasos alternativos:
    echo.
    echo 1. Cierre completamente la aplicación si está abierta
    echo 2. Inicie la aplicación nuevamente
    echo 3. Las tablas se crearán automáticamente
    echo.
    pause
    exit /b 0
)

echo sqlite3.exe encontrado
echo.
echo Ejecutando script SQL...
echo.

sqlite3 "%DB_PATH%" < CrearTablasVentas.sql

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo  ? ACTUALIZACIÓN COMPLETADA
    echo ========================================
    echo.
    echo Las tablas se crearon exitosamente.
    echo Ahora puede iniciar la aplicación y cerrar mesas.
    echo Las ventas se guardarán permanentemente.
    echo.
) else (
    echo.
    echo ========================================
    echo  X ERROR EN LA ACTUALIZACIÓN
    echo ========================================
    echo.
    echo Hubo un problema al ejecutar el script.
    echo Por favor, siga las instrucciones manuales arriba.
    echo.
)

pause
