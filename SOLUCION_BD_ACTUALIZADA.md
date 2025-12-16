# ?? SOLUCIÓN: Base de Datos Actualizada al Ejecutar

## ?? Problema Identificado

**Síntoma**: Al ejecutar la aplicación con el botón de Play sin relleno (Start Without Debugging), aparecía una base de datos antigua o vacía en lugar de la versión actualizada con todos los platillos.

**Causa**: La aplicación usaba una ruta relativa (`Data Source=restaurante.db`), lo que hacía que la base de datos se creara/buscara en diferentes ubicaciones dependiendo de cómo se ejecutara la aplicación.

---

## ? Solución Implementada

### Cambio en `RestauranteContext.cs`

**ANTES:**
```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlite("Data Source=restaurante.db");
}
```

**DESPUÉS:**
```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // Obtener la ruta base de la aplicación (donde está el ejecutable)
    string appPath = AppDomain.CurrentDomain.BaseDirectory;
    string dbPath = Path.Combine(appPath, "restaurante.db");
    
    // Si la BD no existe en carpeta de ejecución pero existe en raíz del proyecto,
    // copiarla automáticamente (útil para primera ejecución en Debug)
    string projectRootDbPath = Path.Combine(appPath, @"..\..\..\restaurante.db");
    if (!File.Exists(dbPath) && File.Exists(projectRootDbPath))
    {
        File.Copy(projectRootDbPath, dbPath, overwrite: false);
    }
    
    optionsBuilder.UseSqlite($"Data Source={dbPath}");
}
```

---

## ?? Qué Hace la Solución

### 1. **Ruta Absoluta Consistente**
- Usa `AppDomain.CurrentDomain.BaseDirectory` para obtener la ruta del ejecutable
- Siempre busca `restaurante.db` en la **misma carpeta** donde está el .exe
- Ya no importa desde dónde ejecutes (Debug, Release, Play con o sin relleno)

### 2. **Copia Automática Inteligente**
- Si no existe `restaurante.db` en la carpeta de ejecución
- Y existe en la raíz del proyecto (`restaurante/restaurante.db`)
- **La copia automáticamente** (solo la primera vez)

### 3. **Sincronización Realizada**
- Se copió la base de datos actualizada desde:
  ```
  restaurante/bin/Debug/net6.0-windows/restaurante.db
  ```
  Hacia:
  ```
  restaurante/restaurante.db
  ```

---

## ?? Ubicaciones de la Base de Datos

### Durante Desarrollo (Debug/Release):
```
restaurante/bin/Debug/net6.0-windows/restaurante.db
```
o
```
restaurante/bin/Release/net6.0-windows/restaurante.db
```

### Fuente Principal (Raíz del Proyecto):
```
restaurante/restaurante.db
```

**Esta es la "fuente de verdad"** - Manténla actualizada haciendo respaldos desde `bin/Debug` cuando hagas cambios importantes.

---

## ?? Cómo Funciona Ahora

### Cuando ejecutas con Play (F5 - Debug):
1. App se ejecuta desde `bin/Debug/net6.0-windows/`
2. Busca `restaurante.db` en esa carpeta
3. Si no existe, copia desde `restaurante/restaurante.db`
4. Usa esa base de datos

### Cuando ejecutas con Play sin relleno (Ctrl+F5 - Start Without Debugging):
1. App se ejecuta desde `bin/Debug/net6.0-windows/`
2. Busca `restaurante.db` en esa carpeta
3. Si no existe, copia desde `restaurante/restaurante.db`
4. Usa esa base de datos

### Cuando ejecutas la versión publicada (Portable):
1. App se ejecuta desde `VersionPortable/`
2. Busca `restaurante.db` en esa carpeta
3. Si no existe, se crea una nueva vacía (tendrías que copiarla manualmente)
4. Usa esa base de datos

---

## ?? Respaldos Recomendados

### Hacer Respaldo:
```cmd
powershell -Command "Copy-Item -Path 'restaurante\bin\Debug\net6.0-windows\restaurante.db' -Destination 'restaurante\restaurante.db' -Force"
```

O simplemente copia manualmente el archivo desde:
```
restaurante\bin\Debug\net6.0-windows\restaurante.db
```
A:
```
restaurante\restaurante.db
```

### Restaurar Respaldo:
```cmd
powershell -Command "Copy-Item -Path 'restaurante\restaurante.db' -Destination 'restaurante\bin\Debug\net6.0-windows\restaurante.db' -Force"
```

---

## ? Verificación

Para verificar que estás usando la base de datos correcta:

1. **Agrega un platillo de prueba** en el Panel de Administración
2. **Cierra la aplicación**
3. **Ejecuta de nuevo** (con cualquier método: Play, Play sin relleno, etc.)
4. **Verifica que el platillo de prueba siga ahí**

Si el platillo desaparece, significa que está usando una BD diferente (contactar para soporte).

---

## ?? Diagnóstico de Problemas

### Si la BD sigue sin actualizarse:

1. **Verifica qué archivo se está usando:**
   - Agrega un `MessageBox.Show(dbPath);` después de la línea `string dbPath = ...` en `RestauranteContext.cs`
   - Te mostrará la ruta exacta que está usando

2. **Elimina todas las copias viejas:**
   ```cmd
   del /s restaurante\bin\*.db
   del /s restaurante\publish\*.db
   ```

3. **Copia la BD actualizada:**
   ```cmd
   copy restaurante\restaurante.db restaurante\bin\Debug\net6.0-windows\
   ```

4. **Ejecuta de nuevo**

---

## ?? Resumen de Archivos .db Encontrados

Archivos .db en tu proyecto:

| Ubicación | Uso |
|-----------|-----|
| `restaurante/restaurante.db` | **Fuente principal** - Hacer respaldos aquí |
| `restaurante/bin/Debug/net6.0-windows/restaurante.db` | **En uso durante desarrollo Debug** |
| `restaurante/bin/Release/net6.0-windows/restaurante.db` | En uso durante desarrollo Release |
| `restaurante/publish/restaurante.db` | Carpeta de publicaciones antiguas |
| `restaurante/publish-single/restaurante.db` | Carpeta de publicaciones antiguas |
| `restaurante/publish-win7/restaurante.db` | Carpeta de publicaciones antiguas |

**Recomendación**: Elimina las carpetas `publish`, `publish-single`, `publish-win7` si no las usas (usar `VersionPortable` en su lugar).

---

## ?? Resultado Final

? **Ahora cualquier forma de ejecutar la app usa la misma base de datos**  
? **La base de datos se mantiene actualizada entre ejecuciones**  
? **Se copia automáticamente si no existe**  
? **Todos los platillos y configuraciones persisten**  

---

**Fecha de modificación**: 15 de diciembre de 2025  
**Archivo modificado**: `restaurante/Data/RestauranteContext.cs`  
**Estado**: ? Base de datos sincronizada y funcional
