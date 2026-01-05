@echo off
echo ========================================
echo   SOLUCION AL ERROR DE IMPRESORA
echo ========================================
echo.
echo Este script actualiza la base de datos para agregar
echo las columnas necesarias para la funcionalidad de
echo "recordar impresora".
echo.
echo Presione cualquier tecla para continuar...
pause >nul

cd /d "%~dp0"
echo.
echo Ejecutando actualizador...
echo.

ActualizadorDB\bin\Debug\net6.0\ActualizadorDB.exe

echo.
echo ========================================
echo   PROCESO COMPLETADO
echo ========================================
echo.
echo Si no hubo errores, la aplicacion deberia
echo funcionar correctamente ahora.
echo.
pause
