# ?? GUÍA RÁPIDA - BASE DE DATOS

## ?? Ubicaciones de la Base de Datos

### Durante Desarrollo:
```
restaurante/bin/Debug/net6.0-windows/restaurante.db
```
Esta es la que usa la aplicación cuando la ejecutas desde Visual Studio.

### Respaldo Principal:
```
restaurante/restaurante.db
```
Esta es tu copia de seguridad principal en la raíz del proyecto.

---

## ?? Ejecución de la Aplicación

### Cualquier método funciona igual:
- ? Play con depuración (F5) - Botón verde relleno
- ? Play sin depuración (Ctrl+F5) - Botón verde sin relleno
- ? Ejecutar el .exe directamente

**Todos usan la misma base de datos** en `bin/Debug/net6.0-windows/`

---

## ?? Scripts de Respaldo

### ?? Hacer Respaldo (Debug ? Raíz del Proyecto)
```cmd
RespaldarBaseDatos.bat
```
Copia desde `bin/Debug/net6.0-windows/restaurante.db` ? `restaurante/restaurante.db`

**¿Cuándo usar?**
- Después de agregar platillos importantes
- Antes de hacer cambios grandes
- Diariamente si usas el sistema en producción

### ?? Restaurar Respaldo (Raíz del Proyecto ? Debug)
```cmd
RestaurarBaseDatos.bat
```
Copia desde `restaurante/restaurante.db` ? `bin/Debug/net6.0-windows/restaurante.db`

**¿Cuándo usar?**
- Si borraste accidentalmente la BD de Debug
- Si quieres volver a una versión anterior
- Si la BD se corrompió

---

## ?? Copia Automática

El sistema ahora incluye **copia automática inteligente**:

1. Al ejecutar la aplicación, verifica si existe `restaurante.db` en la carpeta de ejecución
2. Si NO existe, busca en la raíz del proyecto (`restaurante/restaurante.db`)
3. Si la encuentra ahí, **la copia automáticamente**
4. Esto solo pasa la primera vez

**Beneficio**: No necesitas hacer nada manualmente la primera vez que ejecutas en Debug.

---

## ? Verificación Rápida

### Para verificar que todo funciona:

1. **Abre la aplicación** (con cualquier método)
2. **Panel de Administración** ? **Platillos**
3. **Verifica que veas todos tus platillos** (debería haber más de 150)
4. **Agrega un platillo de prueba**
5. **Cierra la aplicación**
6. **Abre de nuevo**
7. **Verifica que el platillo de prueba sigue ahí**

Si el platillo desaparece ? Hay un problema (contactar para soporte)

---

## ??? Protección de Datos

### ? Lo que está protegido:
- No se borra la BD al ejecutar
- No se recrean datos semilla
- Todos los cambios se guardan permanentemente
- Copia automática desde respaldo si es necesario

### ?? Lo que NO está protegido:
- Borrar manualmente archivos .db
- Ejecutar comandos de migraciones de Entity Framework sin respaldo
- Corrupción del archivo por cierre inesperado

---

## ?? Datos Actuales

Tu base de datos contiene:

- ? **16 categorías** de platillos
- ? **Más de 150 platillos** con precios
- ? **14 mesas** (Mesa 1-13 + Mesa "Para Llevar")
- ? **Configuración del restaurante** (nombre, dirección, teléfono)

---

## ?? Solución de Problemas

### Problema: "No veo mis platillos"
**Solución**:
1. Cierra la aplicación
2. Ejecuta `RestaurarBaseDatos.bat`
3. Abre la aplicación de nuevo

### Problema: "La aplicación no inicia"
**Solución**:
1. Verifica que exista `restaurante/restaurante.db`
2. Si no existe, necesitas una copia de respaldo
3. O crea una nueva BD desde el Panel de Administración

### Problema: "Cambios no se guardan"
**Solución**:
1. Verifica que tengas permisos de escritura en la carpeta
2. Ejecuta Visual Studio como administrador
3. Verifica que no haya otro proceso usando la BD

---

## ?? Archivos de Documentación

- **`PROTECCION_BASE_DATOS.md`** - Cómo se protege la BD
- **`SOLUCION_BD_ACTUALIZADA.md`** - Solución al problema de BD desactualizada
- **`RespaldarBaseDatos.bat`** - Script para hacer respaldos
- **`RestaurarBaseDatos.bat`** - Script para restaurar desde respaldo

---

## ?? Recomendaciones Finales

1. ? **Haz respaldos diarios** si usas el sistema en producción
2. ? **Copia `restaurante.db` a Google Drive/OneDrive** regularmente
3. ? **Antes de actualizar el código**, haz un respaldo
4. ? **Prueba los scripts de respaldo** al menos una vez
5. ? **No modifiques manualmente** el archivo .db con editores

---

**¡Tu base de datos ahora está protegida y sincronizada!** ?

**Última actualización**: 15 de diciembre de 2025  
**Versión**: .NET 6  
**Estado**: ? Funcionando correctamente
