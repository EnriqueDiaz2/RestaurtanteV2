@echo off
echo ========================================
echo   ACTUALIZACION COMPLETA VERSION PORTABLE
echo   (Sobrescribe TODO, incluyendo BD)
echo ========================================
echo.

REM Cambiar al directorio del script
cd /d "%~dp0"

echo ADVERTENCIA: Este script sobrescribira TODOS los archivos
echo en la carpeta VersionPortable, incluyendo la base de datos.
echo.
set /p confirmar="Desea continuar? (S/N): "
if /i not "%confirmar%"=="S" (
    echo.
    echo Operacion cancelada.
    pause
    exit /b 0
)

echo.
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
    rd /s /q VersionPortable
    mkdir VersionPortable
    
    echo Copiando archivos a VersionPortable...
    xcopy /E /I /Y restaurante\bin\Release\net6.0-windows\win-x64\publish\* VersionPortable\
    
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
    echo Tamano total:
    powershell -command "(Get-ChildItem -Path 'VersionPortable' -Recurse | Measure-Object -Property Length -Sum).Sum / 1MB" 2>nul && echo MB
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
