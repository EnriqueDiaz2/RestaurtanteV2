# ?? Sistema POS Restaurante - Versión Portable

## ? Requisitos del Sistema

- **Sistema Operativo**: Windows 10 o superior (64-bit)
- **Espacio en disco**: Mínimo 200 MB
- **RAM**: Mínimo 2 GB
- **NO requiere instalación de .NET Framework o .NET Runtime**

## ?? Instrucciones de Uso

### Primera Ejecución

1. **Copiar la carpeta completa** `PORTABLE_POS` a la computadora donde quieras usar el sistema
2. **Ubicar el archivo** `restaurante.exe` dentro de la carpeta
3. **Doble clic** en `restaurante.exe` para ejecutar el sistema
4. **IMPORTANTE**: Al ejecutar por primera vez, Windows puede mostrar un mensaje de seguridad:
   - Hacer clic en "Más información"
   - Luego hacer clic en "Ejecutar de todas formas"

### Base de Datos

- La base de datos SQLite se crea automáticamente en la primera ejecución
- El archivo se llama `restaurante.db` y se genera en la misma carpeta del ejecutable
- **IMPORTANTE**: NO borrar el archivo `restaurante.db` si quieres conservar tus datos

## ?? Configuración Inicial

Al abrir el sistema por primera vez:

1. **Ir a Admin** (botón morado)
2. **Pestaña "Configuración"**:
   - Nombre del Restaurante
   - Dirección
   - Teléfono
3. **Pestaña "Mesas"**:
   - Agregar las mesas necesarias
   - Puedes agregar múltiples mesas (ejemplo: 1-20)
4. **Pestaña "Categorías"**:
   - Crear categorías de platillos
   - Asignar colores personalizados
5. **Pestaña "Platillos"**:
   - Agregar los platillos del menú
   - Asignar precios y categorías

## ?? Funcionalidades Principales

### ?? Gestión de Mesas
- Ver estado de todas las mesas (Libre/Ocupada)
- Abrir mesas para tomar pedidos
- Cerrar mesas al finalizar
- Pedidos "Para Llevar"

### ??? Toma de Pedidos
- Selección rápida por categorías
- Agregar platillos con cantidades
- Ver total en tiempo real
- Aplicar descuentos (% o $)
- Guardar pedidos en mesas
- Modificar pedidos existentes

### ??? Impresión de Tickets
- Vista previa antes de imprimir
- Incluye todos los detalles del pedido
- Información del mesero
- Fecha y hora
- Total y descuentos

### ?? Panel de Administración
- **Configuración**: Datos del restaurante
- **Categorías**: Gestión con colores personalizados (30 colores disponibles)
- **Platillos**: CRUD completo de platillos
- **Por Categoría**: Vista filtrada de platillos
- **Mesas**: Agregar/Eliminar mesas

## ?? Paleta de Colores para Categorías

30 colores disponibles:
- ?? Azul, ?? Verde, ?? Rojo, ?? Amarillo
- ?? Naranja, ?? Morado, ?? Café, ? Gris Oscuro
- ?? Naranja Oscuro, ?? Verde Esmeralda, ?? Azul Claro
- ?? Rosa, ?? Violeta, ?? Rosa Claro, ? Gris Medio
- Y 15 colores más...

## ?? Estructura de Archivos

```
PORTABLE_POS/
?
??? restaurante.exe          ? Ejecutable principal
??? restaurante.db           ? Base de datos (se crea al ejecutar)
??? Microsoft.*.dll          ? Librerías necesarias
??? System.*.dll             ? Librerías del sistema
??? otros archivos .dll      ? Dependencias
```

## ?? Respaldo de Datos

Para hacer un respaldo de tus datos:

1. **Copiar el archivo** `restaurante.db`
2. **Guardarlo en un lugar seguro** (USB, nube, etc.)
3. Para restaurar: reemplazar el archivo `restaurante.db` con tu respaldo

## ?? Solución de Problemas

### El programa no inicia
- Verificar que tengas Windows 10 o superior (64-bit)
- Asegurarte de que la carpeta completa esté copiada
- Ejecutar como Administrador (clic derecho ? "Ejecutar como administrador")

### Windows Defender bloquea la ejecución
- Es normal en aplicaciones sin firma digital
- Hacer clic en "Más información" ? "Ejecutar de todas formas"

### Perdí mis datos
- Si tienes el archivo `restaurante.db`, tus datos están seguros
- Copiar ese archivo a la nueva instalación

### La aplicación se ve borrosa
- Clic derecho en `restaurante.exe` ? Propiedades
- Pestaña "Compatibilidad"
- Marcar "Invalidar comportamiento de ajuste de PPP"
- Seleccionar "Sistema (Mejorado)"

## ?? Características Técnicas

- **Plataforma**: .NET 6.0 (Windows)
- **Base de Datos**: SQLite (incluida)
- **Arquitectura**: Self-contained x64
- **Modo**: Single-file executable
- **Tamaño aproximado**: ~80-100 MB

## ?? Actualizaciones

Para actualizar a una nueva versión:

1. **Hacer respaldo** de `restaurante.db`
2. **Reemplazar** el ejecutable y archivos DLL
3. **Copiar de vuelta** tu archivo `restaurante.db`

---

**Versión**: 1.0
**Fecha**: 2024
**Sistema**: POS para Restaurantes

¡Disfruta usando el sistema! ??
