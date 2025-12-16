# ?? VERSIÓN PORTABLE ACTUALIZADA - LISTA PARA LLEVAR

## ? UBICACIÓN DE LA CARPETA

**Ruta completa:**
```
C:\Users\Usuario\source\repos\Restaurtante2.5\VersionPortable\
```

Esta carpeta ya está **LISTA** para copiar a la computadora de origen.

---

## ?? CONTENIDO DE LA CARPETA

### Archivos Totales: **308 archivos**

### Archivos Principales:
- ? **restaurante.exe** (151 KB) - Ejecutable principal
- ? **restaurante.dll** (104 KB) - Biblioteca principal
- ? **restaurante.db** (57 KB) - Base de datos CON TODOS LOS PLATILLOS ACTUALIZADOS
- ? **restaurante.deps.json** - Dependencias
- ? **restaurante.runtimeconfig.json** - Configuración del runtime

### Bibliotecas Incluidas:
- ? **Runtime de .NET 6** (todas las DLLs necesarias)
- ? **Entity Framework Core** (para la base de datos)
- ? **SQLite** (motor de base de datos)
- ? **QuestPDF** (para generar tickets PDF)
- ? **Windows Forms** (para la interfaz gráfica)
- ? Todas las dependencias necesarias

---

## ?? INSTRUCCIONES PARA LLEVAR A LA COMPUTADORA DE ORIGEN

### Opción 1: Copiar por USB (Recomendado)

1. **Conecta una USB** a esta computadora
2. **Copia la carpeta completa** `VersionPortable` a la USB
   ```
   VersionPortable (todo el contenido)
   ```
3. **Lleva la USB** a la computadora de origen
4. **Pega la carpeta** en cualquier ubicación (Ejemplo: `C:\Restaurante\`)
5. **Ejecuta** `restaurante.exe`

### Opción 2: Comprimir y Transferir

1. **Click derecho** en la carpeta `VersionPortable`
2. **Enviar a ? Carpeta comprimida**
3. Se creará **`VersionPortable.zip`**
4. **Transfiere el ZIP** (por USB, red, email, etc.)
5. En la computadora de origen:
   - **Extrae el ZIP**
   - **Ejecuta** `restaurante.exe`

### Opción 3: Red Local

Si ambas computadoras están en la misma red:

1. **Comparte la carpeta** `VersionPortable` en red
2. Desde la computadora de origen, **accede a la carpeta compartida**
3. **Copia la carpeta** completa
4. **Ejecuta** `restaurante.exe`

---

## ?? VERIFICACIÓN ANTES DE COPIAR

Asegúrate de que la carpeta `VersionPortable` contiene:

- [x] **308 archivos** (verificado)
- [x] **restaurante.exe** - Ejecutable principal
- [x] **restaurante.db** - Base de datos actualizada con todos los platillos
- [x] Todas las DLLs necesarias (Entity Framework, SQLite, QuestPDF, etc.)
- [x] Runtime de .NET 6 incluido

---

## ?? CONFIGURACIÓN EN LA COMPUTADORA DE ORIGEN

### NO Requiere Instalación:
- ? **NO** necesitas instalar .NET 6
- ? **NO** necesitas instalar SQL Server
- ? **NO** necesitas instalar nada adicional

### Solo Requiere:
- ? Windows 7 SP1 o superior (64 bits)
- ? Impresora configurada (para imprimir tickets)

---

## ?? CÓMO EJECUTAR EN LA COMPUTADORA DE ORIGEN

### Primera Vez:

1. **Copia la carpeta** `VersionPortable` completa
2. **Ubícala** en cualquier lugar (Ejemplo: `C:\Restaurante\`)
3. **Doble click** en `restaurante.exe`
4. La aplicación arrancará con:
   - ? Todos los platillos (150+)
   - ? Todas las categorías (16)
   - ? Todas las mesas (14)
   - ? Configuración del restaurante completa

### Siguientes Veces:

- Solo **doble click** en `restaurante.exe`
- La base de datos se mantiene actualizada

---

## ?? IMPORTANTE: RESPALDO DE LA BASE DE DATOS

### En la Computadora de Origen:

Después de usar la aplicación, la base de datos se actualiza en:
```
C:\Restaurante\VersionPortable\restaurante.db
```

### Hacer Respaldos Regulares:

**Opción 1: Manual**
```
Copiar restaurante.db ? restaurante_backup_[fecha].db
```

**Opción 2: Automático**
Crea un archivo `.bat` con este contenido:
```batch
@echo off
set FECHA=%date:~-4%%date:~3,2%%date:~0,2%
copy restaurante.db restaurante_backup_%FECHA%.db
echo Respaldo creado: restaurante_backup_%FECHA%.db
pause
```

Guárdalo como `Respaldar.bat` en la misma carpeta y ejecútalo cuando quieras hacer un respaldo.

---

## ?? DATOS INCLUIDOS EN LA BASE DE DATOS

### ? Ya incluye:

- **16 Categorías:**
  - Camarones, Filetes, Postres, Bebidas, Cervezas, Desayunos, Cocteles, Entradas, Refrescos, Otros Platillos, Platillos Para Llevar, Licores, Vinos De Mesa, Medias Ord Camaron, Medias Ord Filetes, Dulces

- **Más de 150 Platillos** con:
  - Nombre completo
  - Nombre corto (para tickets)
  - Precio actualizado
  - Categoría asignada

- **14 Mesas:**
  - Mesa 1 a 13 (para servicio en local)
  - Mesa especial "Para Llevar" (para pedidos para llevar)

- **Configuración del Restaurante:**
  - Nombre: Mariscos Pulido
  - Dirección: F. Villa #22 Buenavista, Jal.
  - Teléfono: 3857333334

---

## ?? MEJORAS INCLUIDAS EN ESTA VERSIÓN

### ? Ticket Optimizado:
- Letras más oscuras (fuente en negrita tamaño 10)
- No se corta el contenido (ancho optimizado a 32 caracteres)
- Margen izquierdo reducido (5px en lugar de 10px)
- Perfecto para impresoras térmicas de 58mm/80mm

### ? Base de Datos Protegida:
- No se recrea al iniciar la aplicación
- No se borran datos existentes
- Todos los cambios se guardan permanentemente
- Copia automática desde respaldo si es necesario

### ? Sincronización Mejorada:
- Siempre usa la misma base de datos
- No importa desde dónde ejecutes (Debug o Release)
- Funciona igual en desarrollo y producción

---

## ?? SOLUCIÓN DE PROBLEMAS

### Problema: "La aplicación no arranca"

**Causa**: Falta algún archivo DLL
**Solución**: 
1. Asegúrate de copiar la carpeta **completa** con los 308 archivos
2. No muevas archivos individuales, mueve toda la carpeta

### Problema: "No veo mis platillos"

**Causa**: Se está usando una base de datos diferente
**Solución**:
1. Verifica que el archivo `restaurante.db` esté en la misma carpeta que `restaurante.exe`
2. Tamaño del archivo debe ser aproximadamente **57 KB**
3. Si es más pequeño, copia de nuevo desde esta computadora

### Problema: "El ticket se corta"

**Causa**: Impresora con configuración incorrecta
**Solución**:
1. Verifica que la impresora esté configurada como "Impresora térmica"
2. Ajusta el ancho de papel en la configuración de la impresora
3. El ticket está optimizado para 58mm y 80mm

### Problema: "Las letras se ven claras"

**Causa**: Configuración de calidad de impresión
**Solución**:
1. Panel de Control ? Impresoras ? [Tu Impresora] ? Preferencias
2. Calidad: Alta/Máxima
3. Densidad: Oscuro/Máximo
4. Ahorro de tinta: Desactivado

---

## ?? ESTRUCTURA DE LA CARPETA VersionPortable

```
VersionPortable/
??? restaurante.exe          ? EJECUTAR ESTE ARCHIVO
??? restaurante.dll
??? restaurante.db           ? BASE DE DATOS (IMPORTANTE!)
??? restaurante.deps.json
??? restaurante.runtimeconfig.json
??? [307 archivos DLL más]   ? Runtime y bibliotecas
```

---

## ? CHECKLIST FINAL

Antes de desconectar la USB o transferir:

- [ ] Carpeta `VersionPortable` copiada completa
- [ ] 308 archivos verificados
- [ ] `restaurante.exe` presente
- [ ] `restaurante.db` presente (57 KB)
- [ ] Carpeta en lugar accesible en la PC de origen
- [ ] Primer arranque exitoso
- [ ] Platillos visibles en el sistema
- [ ] Impresora configurada y probada

---

## ?? RESUMEN RÁPIDO

1. **Copia** la carpeta `VersionPortable` completa
2. **Llévala** a la computadora de origen (USB, red, etc.)
3. **Pégala** en cualquier ubicación
4. **Ejecuta** `restaurante.exe`
5. **¡Listo!** Sistema funcionando con todos los datos

---

## ?? INFORMACIÓN ADICIONAL

**Tamaño total de la carpeta**: ~188 MB

**Compatibilidad**:
- Windows 7 SP1 o superior
- Arquitectura: 64 bits (x64)
- No requiere instalación adicional

**Actualizaciones**:
- Para actualizar, simplemente reemplaza la carpeta completa
- IMPORTANTE: Haz respaldo de `restaurante.db` antes de actualizar

---

**Fecha de generación**: 15 de diciembre de 2025  
**Versión**: Portable con optimizaciones de ticket  
**Estado**: ? Lista para usar

---

## ?? ¡LA CARPETA YA ESTÁ LISTA!

La ventana del explorador de archivos se abrió mostrando la carpeta `VersionPortable`.

**Ahora puedes:**
1. Copiarla a tu USB
2. Llevarla a la computadora de origen
3. Ejecutar `restaurante.exe`

**¡Todo funcionará perfectamente!** ?
