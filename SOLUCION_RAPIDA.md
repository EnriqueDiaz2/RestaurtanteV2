# ?? SOLUCIÓN RÁPIDA: No se encuentran ventas del día

## ? EL PROBLEMA
Las tablas `VentasCerradas` y `DetallesVentasCerradas` no existen en su base de datos.

## ? SOLUCIÓN MÁS SIMPLE (Recomendada)

### Opción 1: Usar DB Browser for SQLite

1. **Descargue DB Browser for SQLite**
   - Vaya a: https://sqlitebrowser.org/
   - Descargue e instale la versión para Windows

2. **Abra su base de datos**
   - Abra DB Browser for SQLite
   - Menú: Archivo ? Abrir base de datos
   - Navegue a su proyecto y busque el archivo `restaurante.db`
   - Puede estar en: `restaurante\bin\Debug\net6.0-windows\restaurante.db`

3. **Ejecute el script SQL**
   - Haga clic en la pestaña "Ejecutar SQL"
   - Copie y pegue todo el contenido del archivo `CrearTablasVentas.sql`
   - Haga clic en el botón "? Ejecutar" (o presione F5)
   - Debe ver mensajes de éxito

4. **Guarde los cambios**
   - Menú: Archivo ? Escribir cambios (o Ctrl+S)
   - O haga clic en el botón "Escribir cambios"

5. **Cierre y reinicie su aplicación**
   - Ahora debería funcionar correctamente

---

### Opción 2: Ejecutar el Script Batch

1. **Haga doble clic** en el archivo: `ActualizarBaseDatos.bat`
2. El script buscará automáticamente su base de datos
3. Si encuentra sqlite3, ejecutará el script
4. Si no, le dirá qué hacer

---

### Opción 3: Copiar manualmente el SQL

1. **Abra el archivo** `CrearTablasVentas.sql`
2. **Copie todo** el contenido (Ctrl+A, Ctrl+C)
3. Use cualquier herramienta de SQLite para ejecutarlo:
   - DB Browser for SQLite (recomendado)
   - SQLiteStudio
   - sqlite3.exe desde línea de comandos
   - VS Code con extensión SQLite

---

## ?? CONTENIDO DEL SCRIPT SQL (CrearTablasVentas.sql)

Si quiere copiar y pegar directamente:

```sql
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

CREATE TABLE IF NOT EXISTS DetallesVentasCerradas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    VentaCerradaId INTEGER NOT NULL,
    NombrePlatillo TEXT NOT NULL,
    Cantidad INTEGER NOT NULL,
    PrecioUnitario REAL NOT NULL,
    Total REAL NOT NULL,
    FOREIGN KEY (VentaCerradaId) REFERENCES VentasCerradas(Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_DetallesVentasCerradas_VentaCerradaId 
ON DetallesVentasCerradas(VentaCerradaId);
```

---

## ?? CÓMO VERIFICAR QUE FUNCIONÓ

Después de ejecutar el script SQL:

1. **Abra su aplicación del restaurante**
2. **Seleccione una mesa** o "Para llevar"
3. **Agregue algunos productos**
4. **Guarde el pedido** (botón "?? GUARDAR PEDIDO")
5. **Cierre la mesa** (botón "?? Cerrar Mesa")
6. **Abra "?? CERRAR DÍA"**
7. **¡Debería ver la venta!**

---

## ? PREGUNTAS FRECUENTES

**P: ¿Dónde está mi archivo restaurante.db?**
R: Busque en estas ubicaciones:
- `restaurante\bin\Debug\net6.0-windows\restaurante.db`
- `restaurante\bin\Release\net6.0-windows\win-x64\restaurante.db`
- En la carpeta raíz del proyecto: `restaurante.db`

**P: No tengo DB Browser for SQLite, ¿hay otra forma?**
R: Sí, puede usar:
- SQLiteStudio: https://sqlitestudio.pl/
- Visual Studio Code con la extensión "SQLite"
- sqlite3.exe desde línea de comandos

**P: Ejecuté el script pero sigue sin funcionar**
R: Asegúrese de:
1. Cerrar completamente la aplicación antes de ejecutar el script
2. Ejecutar el script en la base de datos correcta
3. Guardar los cambios después de ejecutar el script
4. Reiniciar la aplicación

**P: ¿Perderé mis datos?**
R: No, el script solo CREA nuevas tablas si no existen. No elimina ni modifica datos existentes.

---

## ?? SI NADA FUNCIONA

Si después de intentar todo lo anterior el problema persiste:

1. **Haga una copia de seguridad** de su archivo `restaurante.db`
2. **Elimine** el archivo `restaurante.db`
3. **Reinicie la aplicación**
4. Se creará una nueva base de datos con todas las tablas necesarias
5. (Nota: Perderá los datos actuales, por eso haga backup primero)

---

## ?? SOPORTE

Si necesita ayuda adicional, por favor proporcione:
- Captura de pantalla del error
- Ubicación de su archivo restaurante.db
- Sistema operativo y versión
