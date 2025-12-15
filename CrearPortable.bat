@echo off
echo ========================================
echo Creando version PORTABLE del Sistema POS
echo ========================================
echo.

echo Limpiando builds anteriores...
if exist restaurante\bin\Release rd /s /q restaurante\bin\Release
if exist restaurante\obj rd /s /q restaurante\obj

echo.
echo Publicando aplicacion portable...
echo Esta operacion puede tardar unos minutos...
echo.

dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=false /p:DebugType=None /p:DebugSymbols=false

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo EXITO! Aplicacion portable creada
    echo ========================================
    echo.
    echo Ubicacion: restaurante\bin\Release\net6.0-windows\win-x64\publish\
    echo.
    echo Archivos principales:
    dir /b restaurante\bin\Release\net6.0-windows\win-x64\publish\*.exe
    echo.
    echo Puedes copiar toda la carpeta 'publish' a otra computadora
    echo.
    
    echo Creando carpeta de distribucion...
    if exist PORTABLE_POS rd /s /q PORTABLE_POS
    mkdir PORTABLE_POS
    
    echo Copiando archivos necesarios...
    xcopy /E /I /Y restaurante\bin\Release\net6.0-windows\win-x64\publish\* PORTABLE_POS\
    
    echo Copiando README...
    copy PORTABLE_README.md PORTABLE_POS\LEEME.txt
    
    echo.
    echo ========================================
    echo Carpeta PORTABLE_POS creada exitosamente!
    echo ========================================
    echo.
    echo Contenido listo para copiar a otra computadora
    echo.
) else (
    echo.
    echo ========================================
    echo ERROR! Hubo un problema al crear la version portable
    echo ========================================
    echo.
)

echo.
echo Presiona cualquier tecla para continuar...
pause >nul
