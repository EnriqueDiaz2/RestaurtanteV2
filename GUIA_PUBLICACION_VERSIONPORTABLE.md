# ?? GUÍA DE PUBLICACIÓN - VERSIÓN PORTABLE

## ?? Descripción

Esta guía explica cómo usar los scripts de publicación para crear la versión portable del Sistema de Restaurante en una carpeta dedicada llamada `VersionPortable`.

---

## ?? Scripts Disponibles

### 1. **PublicarPortable.bat**
   - **Función**: Crea la versión portable en la carpeta `VersionPortable\`
   - **Resultado**: Carpeta con todos los archivos necesarios
   - **Uso**: Para desarrollo o pruebas locales

### 2. **PublicarPortableZIP.bat**
   - **Función**: Crea la versión portable Y la comprime en un archivo ZIP
   - **Resultado**: Carpeta `VersionPortable\` + archivo ZIP con fecha
   - **Uso**: Para distribuir a usuarios finales

---

## ?? Cómo Usar

### Opción 1: Solo Carpeta Portable (Para pruebas)

1. Ejecuta el archivo: **`PublicarPortable.bat`**
2. El script realizará:
   - Limpieza de versión anterior
   - Publicación con `dotnet publish`
   - Creación de archivo `LEEME.txt`
3. Resultado: Carpeta `VersionPortable\` lista para usar

**Ubicación del ejecutable**: `VersionPortable\restaurante.exe`

---

### Opción 2: Carpeta + ZIP (Para distribuir)

1. Ejecuta el archivo: **`PublicarPortableZIP.bat`**
2. El script realizará:
   - Limpieza de versión anterior
   - Publicación con `dotnet publish`
   - Creación de archivo `LEEME.txt`
   - Compresión en archivo ZIP con fecha
3. Resultado: 
   - Carpeta `VersionPortable\`
   - Archivo `SistemaRestaurante_Portable_YYYY-MM-DD.zip`

**Para distribuir**: Comparte el archivo ZIP generado

---

## ?? Detalles del Comando dotnet

Los scripts ejecutan el siguiente comando:

```bash
dotnet publish restaurante\restaurante.csproj ^
    -c Release ^
    -r win-x64 ^
    --self-contained true ^
    -p:PublishSingleFile=false ^
    -p:PublishTrimmed=false ^
    -p:PublishReadyToRun=false ^
    -o VersionPortable
```

### Parámetros Explicados:

| Parámetro | Descripción |
|-----------|-------------|
| `-c Release` | Compilación en modo Release (optimizada) |
| `-r win-x64` | Plataforma destino: Windows 64 bits |
| `--self-contained true` | Incluye el runtime de .NET (no requiere instalación) |
| `-p:PublishSingleFile=false` | Publica como archivos separados (versión portable) |
| `-p:PublishTrimmed=false` | No recorta DLLs (mayor compatibilidad) |
| `-p:PublishReadyToRun=false` | Desactiva optimización ReadyToRun |
| `-o VersionPortable` | **Carpeta de salida**: `VersionPortable\` |

---

## ?? Estructura de la Carpeta Generada

Después de ejecutar los scripts, obtendrás:

```
VersionPortable/
??? restaurante.exe          ? Ejecutable principal
??? restaurante.dll          ? Biblioteca principal
??? LEEME.txt               ? Instrucciones de uso
??? Microsoft.*.dll         ? Bibliotecas de .NET
??? QuestPDF.dll            ? Biblioteca PDF
??? System.*.dll            ? Bibliotecas del sistema
??? [otros archivos DLL]    ? Dependencias necesarias
```

**IMPORTANTE**: 
- **NO eliminar** archivos DLL
- La carpeta completa se puede copiar a cualquier PC con Windows
- El archivo `restaurante.db` se creará automáticamente al ejecutar

---

## ? Ventajas de la Carpeta VersionPortable

1. **Ubicación Clara**: No se mezcla con archivos de desarrollo
2. **Fácil de Distribuir**: Carpeta completa o ZIP
3. **Portable**: Copiar y usar en cualquier PC
4. **No requiere instalación**: Todo incluido
5. **Mantiene datos**: `restaurante.db` se crea en la misma carpeta

---

## ?? Solución de Problemas

### Problema: "dotnet no se reconoce como comando"
**Solución**: 
- Instalar .NET 6 SDK desde: https://dotnet.microsoft.com/download/dotnet/6.0
- Reiniciar la terminal

### Problema: "Error al publicar"
**Solución**:
- Verificar que no haya errores de compilación
- Ejecutar primero: `dotnet build restaurante\restaurante.csproj`

### Problema: "No se puede crear el ZIP"
**Solución**:
- Verificar que PowerShell esté disponible
- Ejecutar solo `PublicarPortable.bat` y comprimir manualmente

---

## ?? Comparación de Scripts

| Característica | PublicarPortable.bat | PublicarPortableZIP.bat |
|----------------|---------------------|------------------------|
| Crea carpeta portable | ? Sí | ? Sí |
| Crea archivo ZIP | ? No | ? Sí |
| Incluye LEEME.txt | ? Sí | ? Sí |
| Abre carpeta al final | ? Sí | ? Sí |
| Tiempo estimado | ~30 seg | ~45 seg |
| Recomendado para | Desarrollo/Pruebas | Distribución |

---

## ?? Ejemplos de Uso

### Caso 1: Probar en tu PC
```batch
PublicarPortable.bat
cd VersionPortable
restaurante.exe
```

### Caso 2: Enviar a un cliente
```batch
PublicarPortableZIP.bat
? Enviar el archivo SistemaRestaurante_Portable_YYYY-MM-DD.zip
```

### Caso 3: Copiar a USB
```batch
PublicarPortable.bat
? Copiar toda la carpeta VersionPortable\ a la USB
```

---

## ?? Notas Importantes

1. **Tamaño aproximado**: ~60-80 MB (por incluir runtime de .NET)
2. **Tiempo de publicación**: 30-60 segundos
3. **Requisitos del usuario final**: Windows 7 SP1 o superior (64 bits)
4. **Actualización**: Ejecutar de nuevo el script para nueva versión
5. **Respaldo**: Siempre guardar `restaurante.db` antes de actualizar

---

## ?? Diferencia con Otros Scripts

| Script | Carpeta Salida | Tipo de Publicación |
|--------|---------------|---------------------|
| `CrearPortable.bat` | `publish\portable\` | Archivos separados |
| `CrearPortableCompleto.bat` | `publish\portable\` | Archivos + ZIP |
| **`PublicarPortable.bat`** | **`VersionPortable\`** | **Archivos separados** |
| **`PublicarPortableZIP.bat`** | **`VersionPortable\`** | **Archivos + ZIP** |

**Ventaja de VersionPortable**: Carpeta dedicada, fácil de identificar y distribuir.

---

## ?? Siguientes Pasos Después de Publicar

1. **Prueba local**:
   ```
   cd VersionPortable
   restaurante.exe
   ```

2. **Verificar funcionamiento**:
   - ? Abre correctamente
   - ? Crea base de datos
   - ? Administración funciona
   - ? Mesas funcionan
   - ? Tickets PDF funcionan

3. **Distribuir**:
   - Comprimir `VersionPortable\` en ZIP (si usaste PublicarPortable.bat)
   - O usar directamente el ZIP generado (si usaste PublicarPortableZIP.bat)

---

## ? Conclusión

Los scripts `PublicarPortable.bat` y `PublicarPortableZIP.bat` simplifican la creación de la versión portable, colocando todos los archivos en una carpeta dedicada `VersionPortable\` que es:
- ? Fácil de identificar
- ? Lista para distribuir
- ? Portable y autónoma
- ? No requiere instalación

**Recomendación**: Usa `PublicarPortableZIP.bat` para distribución final.

---

**Fecha de creación**: $(Get-Date -Format "yyyy-MM-dd")
**Versión**: 1.0
