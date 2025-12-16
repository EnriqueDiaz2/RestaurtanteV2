# ? CHECKLIST - TRANSFERIR VERSIÓN PORTABLE

## ?? PASOS PARA LLEVAR EL SISTEMA A LA COMPUTADORA DE ORIGEN

---

### ?? PASO 1: PREPARAR LA CARPETA

- [x] ? Carpeta `VersionPortable` generada
- [x] ? 308 archivos verificados
- [x] ? `restaurante.exe` presente (151 KB)
- [x] ? `restaurante.db` presente (57 KB) con todos los platillos
- [x] ? Todas las DLLs incluidas

**Estado**: ? COMPLETADO

---

### ?? PASO 2: COPIAR A USB (O TRANSFERIR)

#### Opción A: Copiar Carpeta Directamente
```
? Conectar USB a esta computadora
? Copiar la carpeta completa "VersionPortable"
? Pegar en la USB
? Verificar que se copiaron los 308 archivos
? Expulsar USB de forma segura
```

#### Opción B: Comprimir Primero (Recomendado para archivos grandes)
```
? Ejecutar ComprimirVersionPortable.bat
? Esperar a que se genere VersionPortable_Restaurante.zip
? Copiar el ZIP a la USB
? Expulsar USB de forma segura
```

**Ventajas de comprimir:**
- ? Archivo más pequeño (~50-70 MB en lugar de ~188 MB)
- ? Transferencia más rápida
- ? Menos probabilidad de errores de copia
- ? Fácil de enviar por email/red si es necesario

---

### ?? PASO 3: LLEVAR A COMPUTADORA DE ORIGEN

```
? Llevar la USB a la computadora de origen
? Conectar la USB
? Abrir la USB en el explorador
```

---

### ?? PASO 4: INSTALAR EN LA COMPUTADORA DE ORIGEN

#### Si Copiaste la Carpeta Directamente:
```
? Crear una carpeta en C:\ (ejemplo: C:\Restaurante\)
? Copiar la carpeta "VersionPortable" completa
? Pegar en C:\Restaurante\
? Esperar a que se copien todos los archivos
? Verificar que se copiaron los 308 archivos
```

#### Si Copiaste el ZIP:
```
? Copiar VersionPortable_Restaurante.zip a C:\
? Click derecho en el ZIP ? Extraer todo...
? Seleccionar destino: C:\Restaurante\
? Click en "Extraer"
? Esperar a que se extraigan todos los archivos
? Verificar que se extrajeron los 308 archivos
```

---

### ?? PASO 5: PRIMERA EJECUCIÓN

```
? Navegar a la carpeta donde quedó la aplicación
   Ejemplo: C:\Restaurante\VersionPortable\
? Buscar el archivo "restaurante.exe"
? Doble click en "restaurante.exe"
? Esperar a que cargue la aplicación
```

**Si aparece advertencia de Windows:**
```
? Click en "Más información"
? Click en "Ejecutar de todas formas"
```

---

### ? PASO 6: VERIFICACIÓN INICIAL

```
? La aplicación se abre correctamente
? Aparece la pantalla principal del POS
? Se pueden ver las categorías de platillos
? Click en "?? ADMIN" ? "?? Categorías"
? Verificar que aparecen las 16 categorías
? Click en "??? Platillos"
? Verificar que aparecen más de 150 platillos
? Verificar que los precios son correctos
? Click en "?? Mesas"
? Verificar que aparecen 13 mesas + "Para Llevar"
```

**Si algo falta:**
- ? Cierra la aplicación
- ? Elimina la carpeta
- ? Vuelve a copiar desde la USB
- ? Verifica que el archivo `restaurante.db` tenga 57 KB

---

### ??? PASO 7: CONFIGURAR IMPRESORA

```
? Conectar la impresora térmica
? Instalar drivers si es necesario
? Configurar como impresora predeterminada (opcional)
? Probar impresión desde el sistema:
   ? Crear un pedido de prueba
   ? Click en "??? IMPRIMIR"
   ? Seleccionar la impresora
   ? Click en "Imprimir"
   ? Verificar que el ticket sale correctamente
   ? Verificar que las letras son legibles
   ? Verificar que no se corta el contenido
```

**Si las letras salen claras:**
```
? Panel de Control ? Impresoras
? Click derecho en tu impresora ? Preferencias
? Calidad: Alta/Máxima
? Densidad: Oscuro/Máximo
? Ahorro de tinta: Desactivado
? Aplicar y probar de nuevo
```

---

### ?? PASO 8: CONFIGURACIÓN DEL RESTAURANTE

```
? Click en "?? ADMIN" ? "?? Configuración"
? Verificar datos:
   ? Nombre: Mariscos Pulido
   ? Dirección: F. Villa #22 Buenavista, Jal.
   ? Teléfono: 3857333334
? Modificar si es necesario
? Click en "?? GUARDAR CONFIGURACIÓN"
```

---

### ?? PASO 9: CREAR RESPALDO

```
? Navegar a C:\Restaurante\VersionPortable\
? Buscar el archivo "restaurante.db"
? Click derecho ? Copiar
? Pegar en la misma carpeta
? Renombrar la copia a "restaurante_backup_[fecha].db"
   Ejemplo: restaurante_backup_15dic2025.db
```

**Crear script de respaldo automático:**
```
? Crear un archivo de texto
? Pegar el código del script (ver INSTRUCCIONES_VERSION_PORTABLE.md)
? Guardar como "Respaldar.bat"
? Ubicar en C:\Restaurante\VersionPortable\
? Probar ejecutándolo
```

---

### ?? PASO 10: PRUEBA COMPLETA DEL SISTEMA

```
? Abrir mesa de prueba (ejemplo: Mesa 1)
? Agregar varios platillos al pedido
? Verificar que los precios se calculan correctamente
? Aplicar un descuento de prueba
? Verificar que el total es correcto
? Guardar el pedido
? Imprimir ticket
? Verificar ticket impreso:
   ? Nombre del restaurante
   ? Dirección y teléfono
   ? Número de mesa
   ? Lista de platillos
   ? Cantidades correctas
   ? Precios correctos
   ? Subtotal correcto
   ? Descuento aplicado (si aplica)
   ? Total correcto
   ? Legibilidad (letras oscuras)
   ? Sin cortes del lado derecho
? Cerrar mesa
? Verificar que la mesa quedó limpia
```

---

### ?? PASO 11: SISTEMA LISTO PARA USAR

```
? Cerrar la aplicación de prueba
? Abrir de nuevo para verificar que los datos persisten
? Configurar acceso directo en el escritorio (opcional):
   ? Click derecho en restaurante.exe
   ? Enviar a ? Escritorio (crear acceso directo)
? Capacitar al personal sobre cómo usar el sistema
? Crear documento con instrucciones básicas
? Configurar respaldos automáticos diarios
```

---

## ?? RESUMEN DE VERIFICACIÓN

### ? Archivos Necesarios:
- [x] restaurante.exe (151 KB)
- [x] restaurante.db (57 KB)
- [x] 306 archivos DLL adicionales
- [x] Total: 308 archivos

### ? Datos Incluidos:
- [x] 16 categorías de platillos
- [x] Más de 150 platillos con precios
- [x] 14 mesas (13 + Para Llevar)
- [x] Configuración del restaurante completa

### ? Funcionalidades Probadas:
- [ ] Abrir mesas
- [ ] Agregar platillos
- [ ] Calcular totales
- [ ] Aplicar descuentos
- [ ] Guardar pedidos
- [ ] Imprimir tickets
- [ ] Cerrar mesas
- [ ] Panel de administración

### ? Configuraciones:
- [ ] Impresora configurada
- [ ] Calidad de impresión óptima
- [ ] Datos del restaurante correctos
- [ ] Respaldo inicial creado

---

## ?? SOLUCIÓN DE PROBLEMAS RÁPIDA

| Problema | Solución Rápida |
|----------|----------------|
| No arranca | Verificar que están los 308 archivos |
| No hay platillos | Verificar que restaurante.db tenga 57 KB |
| Ticket se corta | Verificar configuración de impresora |
| Letras claras | Aumentar densidad en preferencias de impresora |
| Error al guardar | Verificar permisos de escritura en la carpeta |

---

## ?? INFORMACIÓN DE CONTACTO Y SOPORTE

Para más información, consulta estos archivos:
- `INSTRUCCIONES_VERSION_PORTABLE.md` - Instrucciones detalladas
- `OPTIMIZACION_TICKET.md` - Información sobre mejoras del ticket
- `PROTECCION_BASE_DATOS.md` - Cómo está protegida la base de datos
- `README_BASE_DATOS.md` - Guía de la base de datos

---

## ? ESTADO FINAL

Una vez completados todos los pasos:

```
? Sistema instalado
? Datos verificados
? Impresora configurada
? Respaldo creado
? Pruebas completadas
? Personal capacitado
? ¡LISTO PARA PRODUCCIÓN!
```

---

**Fecha**: 15 de diciembre de 2025  
**Versión**: Portable optimizada  
**Estado**: ? Lista para transferir

**¡Todo está listo para llevar a la computadora de origen!** ??
