@echo off
echo ========================================
echo   ACTUALIZACION VERSION PORTABLE
echo   Sistema POS - Restaurante
echo ========================================
echo.

REM Cambiar al directorio del script
cd /d "%~dp0"

REM Verificar si existe la carpeta VersionPortable
if not exist "VersionPortable" (
    echo Creando carpeta VersionPortable...
    mkdir VersionPortable
    echo.
)

echo Limpiando builds anteriores...
if exist restaurante\bin\Release rd /s /q restaurante\bin\Release
if exist restaurante\obj rd /s /q restaurante\obj

echo.
echo Compilando version portable...
echo Esta operacion puede tardar unos minutos...
echo.

REM Compilar la aplicacion en modo Release portable
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=false /p:DebugType=None /p:DebugSymbols=false

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo COMPILACION EXITOSA
    echo ========================================
    echo.
    
    echo Limpiando carpeta VersionPortable...
    REM Hacer backup de restaurante.db si existe
    if exist VersionPortable\restaurante.db (
        echo Haciendo backup de la base de datos...
        copy /Y VersionPortable\restaurante.db VersionPortable\restaurante.db.backup
    )
    
    REM Limpiar todo excepto la base de datos
    for /f "delims=" %%i in ('dir /b "VersionPortable\*" 2^>nul') do (
        if /i not "%%i"=="restaurante.db" (
            if exist "VersionPortable\%%i\" (
                rd /s /q "VersionPortable\%%i"
            ) else (
                del /q "VersionPortable\%%i"
            )
        )
    )
    
    echo Copiando archivos actualizados a VersionPortable...
    xcopy /E /I /Y restaurante\bin\Release\net6.0-windows\win-x64\publish\* VersionPortable\
    
    REM Si hicimos backup, restaurar la base de datos
    if exist VersionPortable\restaurante.db.backup (
        echo Restaurando base de datos existente...
        copy /Y VersionPortable\restaurante.db.backup VersionPortable\restaurante.db
        del /q VersionPortable\restaurante.db.backup
    )
    
    echo.
    echo ========================================
    echo ACTUALIZACION COMPLETADA
    echo ========================================
    echo.
    echo Carpeta: VersionPortable
    echo.
    echo Archivos principales:
    dir /b VersionPortable\*.exe
    echo.
    echo NOTA: La base de datos existente se ha conservado.
    echo       Todos los demas archivos se han actualizado.
    echo.
    echo Ya puede copiar la carpeta VersionPortable
    echo a la computadora de destino.
    echo.
) else (
    echo.
    echo ========================================
    echo ERROR EN LA COMPILACION
    echo ========================================
    echo.
    echo Revise los mensajes de error arriba.
    echo.
)

echo.
pause
