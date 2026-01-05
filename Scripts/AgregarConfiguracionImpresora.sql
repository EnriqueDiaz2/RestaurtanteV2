-- Script para agregar configuración de impresora
-- Este script agrega los campos necesarios para recordar la impresora seleccionada

-- Agregar columna ImpresoraSeleccionada si no existe
ALTER TABLE Configuraciones ADD COLUMN ImpresoraSeleccionada TEXT NULL;

-- Agregar columna ImprimirDirectamente si no existe
ALTER TABLE Configuraciones ADD COLUMN ImprimirDirectamente INTEGER NOT NULL DEFAULT 0;

-- Mensaje de confirmación
SELECT 'Columnas agregadas correctamente a la tabla Configuraciones' AS Resultado;
