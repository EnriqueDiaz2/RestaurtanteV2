-- Script para crear las tablas de Ventas Cerradas
-- Ejecute este script en su base de datos restaurante.db usando SQLite

-- Crear tabla VentasCerradas
CREATE TABLE IF NOT EXISTS VentasCerradas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    NumeroMesa INTEGER NOT NULL,
    FechaApertura TEXT NOT NULL,
    FechaCierre TEXT NOT NULL,
    Mesero TEXT NOT NULL,
    Subtotal REAL NOT NULL,
    Descuento REAL NOT NULL,
    Total REAL NOT NULL
);

-- Crear tabla DetallesVentasCerradas
CREATE TABLE IF NOT EXISTS DetallesVentasCerradas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    VentaCerradaId INTEGER NOT NULL,
    NombrePlatillo TEXT NOT NULL,
    Cantidad INTEGER NOT NULL,
    PrecioUnitario REAL NOT NULL,
    Total REAL NOT NULL,
    FOREIGN KEY (VentaCerradaId) REFERENCES VentasCerradas(Id) ON DELETE CASCADE
);

-- Crear índice para mejorar el rendimiento
CREATE INDEX IF NOT EXISTS IX_DetallesVentasCerradas_VentaCerradaId 
ON DetallesVentasCerradas(VentaCerradaId);

-- Verificar que las tablas se crearon
SELECT 'Tabla VentasCerradas creada' as Mensaje;
SELECT 'Tabla DetallesVentasCerradas creada' as Mensaje;
