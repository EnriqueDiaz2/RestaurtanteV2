@echo off
title Actualizar Version Portable - Sistema POS
color 0A

:MENU
cls
echo ========================================
echo   SISTEMA POS - RESTAURANTE
echo   Actualizador Version Portable
echo ========================================
echo.
echo Carpeta destino: VersionPortable
echo.
echo Seleccione una opcion:
echo.
echo [1] Actualizar (Conservar base de datos)
echo [2] Actualizar TODO (Sobrescribir todo)
echo [3] Solo compilar (No copiar a VersionPortable)
echo [4] Ver informacion de VersionPortable
echo [5] Salir
echo.
set /p opcion="Ingrese su opcion (1-5): "

if "%opcion%"=="1" goto ACTUALIZAR_CONSERVAR
if "%opcion%"=="2" goto ACTUALIZAR_TODO
if "%opcion%"=="3" goto SOLO_COMPILAR
if "%opcion%"=="4" goto VER_INFO
if "%opcion%"=="5" goto SALIR

echo.
echo Opcion invalida. Presione una tecla para continuar...
pause >nul
goto MENU

:ACTUALIZAR_CONSERVAR
cls
echo ========================================
echo   ACTUALIZANDO (Conservando BD)
echo ========================================
echo.

if not exist "VersionPortable" (
    mkdir VersionPortable
)

echo Limpiando builds anteriores...
if exist restaurante\bin\Release rd /s /q restaurante\bin\Release
if exist restaurante\obj rd /s /q restaurante\obj

echo.
echo Compilando...
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=false /p:DebugType=None /p:DebugSymbols=false

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR en la compilacion!
    pause
    goto MENU
)

echo.
echo Respaldando base de datos...
if exist VersionPortable\restaurante.db (
    copy /Y VersionPortable\restaurante.db VersionPortable\restaurante.db.backup
)

echo Limpiando archivos antiguos (excepto BD)...
for /f "delims=" %%i in ('dir /b "VersionPortable\*" 2^>nul') do (
    if /i not "%%i"=="restaurante.db" (
        if /i not "%%i"=="restaurante.db.backup" (
            if exist "VersionPortable\%%i\" (
                rd /s /q "VersionPortable\%%i"
            ) else (
                del /q "VersionPortable\%%i"
            )
        )
    )
)

echo Copiando archivos actualizados...
xcopy /E /I /Y /Q restaurante\bin\Release\net6.0-windows\win-x64\publish\* VersionPortable\

if exist VersionPortable\restaurante.db.backup (
    copy /Y VersionPortable\restaurante.db.backup VersionPortable\restaurante.db
    del /q VersionPortable\restaurante.db.backup
)

echo.
echo ========================================
echo   ACTUALIZACION COMPLETADA
echo ========================================
echo.
echo Base de datos conservada.
echo Presione una tecla para volver al menu...
pause >nul
goto MENU

:ACTUALIZAR_TODO
cls
echo ========================================
echo   ACTUALIZACION COMPLETA
echo ========================================
echo.
echo ADVERTENCIA: Esto sobrescribira TODOS los archivos
echo incluyendo la base de datos existente.
echo.
set /p confirmar="Confirmar? (S/N): "
if /i not "%confirmar%"=="S" (
    echo Operacion cancelada.
    pause
    goto MENU
)

echo.
if not exist "VersionPortable" (
    mkdir VersionPortable
)

echo Limpiando builds anteriores...
if exist restaurante\bin\Release rd /s /q restaurante\bin\Release
if exist restaurante\obj rd /s /q restaurante\obj

echo.
echo Compilando...
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=false /p:DebugType=None /p:DebugSymbols=false

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR en la compilacion!
    pause
    goto MENU
)

echo.
echo Limpiando carpeta VersionPortable...
rd /s /q VersionPortable
mkdir VersionPortable

echo Copiando archivos...
xcopy /E /I /Y /Q restaurante\bin\Release\net6.0-windows\win-x64\publish\* VersionPortable\

echo.
echo ========================================
echo   ACTUALIZACION COMPLETADA
echo ========================================
echo.
echo TODO actualizado (incluyendo BD).
echo Presione una tecla para volver al menu...
pause >nul
goto MENU

:SOLO_COMPILAR
cls
echo ========================================
echo   COMPILANDO
echo ========================================
echo.

echo Limpiando builds anteriores...
if exist restaurante\bin\Release rd /s /q restaurante\bin\Release
if exist restaurante\obj rd /s /q restaurante\obj

echo.
echo Compilando...
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=false /p:DebugType=None /p:DebugSymbols=false

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR en la compilacion!
    pause
    goto MENU
)

echo.
echo ========================================
echo   COMPILACION COMPLETADA
echo ========================================
echo.
echo Ubicacion: restaurante\bin\Release\net6.0-windows\win-x64\publish\
echo.
echo Presione una tecla para volver al menu...
pause >nul
goto MENU

:VER_INFO
cls
echo ========================================
echo   INFORMACION DE VERSION PORTABLE
echo ========================================
echo.

if not exist "VersionPortable" (
    echo La carpeta VersionPortable NO existe.
    echo.
    pause
    goto MENU
)

echo Carpeta: VersionPortable
echo.
echo Archivos principales:
dir /b VersionPortable\*.exe 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [No se encontraron archivos .exe]
)

echo.
echo Base de datos:
if exist VersionPortable\restaurante.db (
    echo   restaurante.db - EXISTE
    for %%A in (VersionPortable\restaurante.db) do echo   Tamano: %%~zA bytes
    for %%A in (VersionPortable\restaurante.db) do echo   Modificado: %%~tA
) else (
    echo   restaurante.db - NO EXISTE
)

echo.
echo Tamano total de la carpeta:
powershell -command "$size = (Get-ChildItem -Path 'VersionPortable' -Recurse -ErrorAction SilentlyContinue | Measure-Object -Property Length -Sum).Sum; Write-Host ([math]::Round($size/1MB, 2)) 'MB'" 2>nul

echo.
echo Cantidad de archivos:
powershell -command "(Get-ChildItem -Path 'VersionPortable' -Recurse -File -ErrorAction SilentlyContinue | Measure-Object).Count" 2>nul

echo.
pause
goto MENU

:SALIR
cls
echo.
echo Saliendo...
echo.
exit /b 0
