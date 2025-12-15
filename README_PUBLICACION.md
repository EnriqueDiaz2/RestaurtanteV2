# ??? Sistema de Restaurante - Publicación Portable

## ?? Crear Versión Portable

Para generar la versión portable del sistema, ejecuta uno de estos scripts:

### ?? Opción 1: Solo Carpeta
```batch
PublicarPortable.bat
```
- Genera la carpeta `VersionPortable\`
- Ideal para pruebas locales

### ?? Opción 2: Carpeta + ZIP (Recomendado)
```batch
PublicarPortableZIP.bat
```
- Genera la carpeta `VersionPortable\`
- Crea archivo ZIP para distribución
- Nombre: `SistemaRestaurante_Portable_YYYY-MM-DD.zip`

## ?? Resultado

Los scripts ejecutan el siguiente comando `dotnet`:

```bash
dotnet publish restaurante\restaurante.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o VersionPortable
```

## ?? Ubicación de Archivos

- **Carpeta de salida**: `VersionPortable\`
- **Ejecutable**: `VersionPortable\restaurante.exe`
- **Archivo ZIP**: `SistemaRestaurante_Portable_YYYY-MM-DD.zip` (si usas PublicarPortableZIP.bat)

## ? Ventajas

- ? Carpeta dedicada y fácil de identificar
- ?? No requiere instalación de .NET
- ?? Funciona en Windows 7 SP1 o superior (64 bits)
- ?? Portable: copiar y usar en cualquier PC
- ?? Incluye todas las dependencias necesarias

## ?? Documentación Completa

Para más detalles, consulta: **`GUIA_PUBLICACION_VERSIONPORTABLE.md`**

---

**Proyecto**: Sistema de Restaurante  
**Framework**: .NET 6  
**Tipo**: Windows Forms Application
