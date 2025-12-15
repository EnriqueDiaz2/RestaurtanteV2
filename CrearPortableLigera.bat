@echo off
echo ========================================
echo Creando version PORTABLE LIGERA del Sistema POS
echo ========================================
echo.
echo Esta version es mas pequena pero requiere que
echo la computadora destino tenga .NET 6 Desktop Runtime
echo.

cd restaurante

echo Limpiando builds anteriores...
if exist bin\Release rd /s /q bin\Release
if exist obj rd /s /q obj

echo.
echo Publicando aplicacion portable ligera...
echo.

dotnet publish -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true /p:DebugType=None /p:DebugSymbols=false

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo EXITO! Aplicacion portable LIGERA creada
    echo ========================================
    echo.
    echo Ubicacion: restaurante\bin\Release\net6.0-windows\win-x64\publish\
    echo.
    
    echo Creando carpeta de distribucion...
    if exist ..\PORTABLE_POS_LIGERA rd /s /q ..\PORTABLE_POS_LIGERA
    mkdir ..\PORTABLE_POS_LIGERA
    
    echo Copiando archivos necesarios...
    xcopy /E /I /Y bin\Release\net6.0-windows\win-x64\publish\* ..\PORTABLE_POS_LIGERA\
    
    echo.
    echo NOTA: Esta version requiere .NET 6 Desktop Runtime
    echo Descarga: https://dotnet.microsoft.com/download/dotnet/6.0
    echo.
    echo Tamano aproximado: ~10-15 MB (vs 80-100 MB de la version completa)
    echo.
) else (
    echo.
    echo ERROR! Hubo un problema al crear la version portable
    echo.
)

cd ..

echo.
echo Presiona cualquier tecla para continuar...
pause >nul
