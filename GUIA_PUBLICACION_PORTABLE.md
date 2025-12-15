# ?? GUÍA DE PUBLICACIÓN PORTABLE

## ?? MÉTODO RÁPIDO - SCRIPTS AUTOMATIZADOS (RECOMENDADO)

### Opción 1: Script TODO EN UNO (Más Fácil)

**Ejecutar:** `CrearPortableCompleto.bat`

Este script hace TODO automáticamente:
1. ? Limpia compilaciones anteriores
2. ?? Compila el proyecto
3. ?? Publica la versión portable
4. ?? Crea el archivo LEEME.txt
5. ??? Comprime todo en un archivo ZIP

**Resultado:** 
- Archivo `Restaurante_Portable_YYYY-MM-DD.zip` listo para distribuir
- Carpeta `publish\portable\` con todos los archivos

---

### Opción 2: Solo Crear ZIP (Si Ya Publicaste)

**Ejecutar:** `CrearZipPortable.bat`

Usa este script si ya tienes la carpeta `publish\portable\` y solo quieres crear el ZIP.

---

## ?? Cómo Publicar la Versión Portable desde Visual Studio

### Método 1: Usar el Perfil de Publicación "PortableProfile"

1. **Abrir el menú de publicación:**
   - Click derecho en el proyecto `restaurante` en el Explorador de Soluciones
   - Seleccionar `Publicar...`

2. **Seleccionar el perfil portable:**
   - En la ventana de publicación, buscar el perfil llamado **"PortableProfile"**
   - Si no aparece, hacer click en `Nuevo` y seleccionar `Carpeta`
   - Nombrar el perfil como "PortableProfile"

3. **Publicar:**
   - Click en el botón `Publicar`
   - Los archivos se generarán en: `restaurante\bin\Release\net6.0-windows\publish\portable\`

4. **Resultado:**
   - Se generarán **TODOS** los archivos separados (DLLs, ejecutable, recursos)
   - Esta carpeta contendrá la versión portable completa
   - Puedes copiar toda la carpeta a cualquier computadora Windows sin instalación

---

### Método 2: Usar la Línea de Comandos (Alternativo)

Abrir terminal en la carpeta del proyecto y ejecutar:

```cmd
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o publish\portable
```

---

## ?? Archivos Generados

La publicación portable generará los siguientes archivos:

### Ejecutable Principal
- `restaurante.exe` - Aplicación principal

### Base de Datos
- `restaurante.db` - Base de datos SQLite (se crea automáticamente al ejecutar)

### DLLs de .NET Runtime
- `*.dll` - Todas las librerías necesarias del runtime de .NET 6
- Archivos nativos: `coreclr.dll`, `clrjit.dll`, `hostfxr.dll`, etc.

### DLLs de Dependencias
- `Microsoft.EntityFrameworkCore*.dll` - Entity Framework
- `Microsoft.Data.Sqlite.dll` - Proveedor SQLite
- `Microsoft.VisualBasic*.dll` - Visual Basic runtime
- `QuestPDF*.dll` - Generación de PDFs
- `e_sqlite3.dll` - SQLite nativo
- `SkiaSharp*.dll` - Renderizado de gráficos para PDF

### Archivos de Recursos
- `es\*.resources.dll` - Recursos en español
- Otros idiomas si están incluidos

### Archivos de Configuración
- `restaurante.deps.json` - Dependencias
- `restaurante.runtimeconfig.json` - Configuración del runtime

### Documentación
- `LEEME.txt` - Instrucciones para el usuario final

---

## ? Diferencias entre Perfiles

| Característica | Release Normal | PortableProfile |
|---------------|----------------|-----------------|
| **Archivo único** | ? Sí (un solo .exe) | ? No (múltiples archivos) |
| **Tamaño** | ~150-200 MB | ~180-220 MB total |
| **Velocidad inicial** | Más lento (descompresión) | Más rápido |
| **Portabilidad** | Alta | Alta |
| **Debugging** | Difícil | Más fácil |
| **Reemplazo DLL** | Imposible | Posible |
| **Recomendado para** | Distribución simple | Desarrollo y soporte técnico |

---

## ?? Distribución

### Método Automático (Recomendado):

1. Ejecutar `CrearPortableCompleto.bat`
2. Obtener el archivo ZIP generado: `Restaurante_Portable_YYYY-MM-DD.zip`
3. Distribuir el ZIP a los usuarios

### Método Manual:

1. Publicar usando el perfil "PortableProfile"
2. Navegar a: `restaurante\bin\Release\net6.0-windows\publish\portable\`
3. Seleccionar todos los archivos
4. Click derecho ? `Comprimir en archivo ZIP`
5. Nombrar como `Restaurante_Portable_vX.X.zip`

### Contenido que recibirá el usuario:
```
Restaurante_Portable_YYYY-MM-DD.zip
??? restaurante.exe            ? Ejecutable principal
??? LEEME.txt                  ? Instrucciones
??? *.dll                      ? Todas las DLLs necesarias
??? *.json                     ? Archivos de configuración
??? es\                        ? Recursos en español
    ??? *.resources.dll
```

### Instrucciones para el Usuario Final:

1. **Descomprimir** el archivo ZIP en cualquier carpeta
2. **Ejecutar** `restaurante.exe` (doble clic)
3. **¡Listo!** No requiere instalación

---

## ?? Solución de Problemas

### Si falta algún archivo:
```cmd
dotnet clean
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o publish\portable
```

### Si aparece error de dependencias:
```cmd
dotnet restore
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o publish\portable
```

### Si el script .bat no funciona:
1. Ejecutar como **Administrador** (click derecho ? "Ejecutar como administrador")
2. Verificar que PowerShell esté habilitado
3. Usar el método manual de línea de comandos

---

## ?? Estadísticas de la Versión Portable

Después de ejecutar `CrearPortableCompleto.bat`, obtendrás:

- **Total de archivos:** ~200-250 archivos
- **Tamaño descomprimido:** ~180-220 MB
- **Tamaño del ZIP:** ~60-80 MB (comprimido)
- **Tiempo de publicación:** 3-5 segundos
- **Tiempo de compresión:** 5-10 segundos

---

## ?? Notas Importantes

- La versión portable **NO requiere instalación de .NET** en la PC de destino
- Incluye todas las DLLs necesarias para funcionar de manera independiente
- Compatible con Windows 7 SP1 o superior (x64)
- La base de datos se crea automáticamente en la primera ejecución
- **IMPORTANTE:** Para actualizar, reemplazar TODOS los archivos EXCEPTO `restaurante.db`

---

## ?? Ventajas de la Versión Portable

? **Sin instalación** - Solo descomprimir y ejecutar  
? **Totalmente autónoma** - No requiere .NET instalado  
? **Portable** - Copiar a USB y ejecutar en cualquier PC  
? **Múltiples instancias** - Cada carpeta es independiente  
? **Fácil respaldo** - Solo copiar la carpeta completa  
? **Actualización simple** - Reemplazar archivos  
? **Sin registro** - No modifica el sistema Windows  

---

## ?? Actualización de Versión Portable

Para actualizar una instalación portable existente:

1. **Respaldar datos:**
   ```
   Copiar restaurante.db a un lugar seguro
   ```

2. **Reemplazar archivos:**
   ```
   Descomprimir la nueva versión sobre la anterior
   ```

3. **Restaurar datos:**
   ```
   Copiar restaurante.db de vuelta (si es necesario)
   ```

**Nota:** La nueva versión debe mantener compatibilidad con la base de datos anterior.
