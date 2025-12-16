# ??? PROTECCIÓN DE LA BASE DE DATOS

## ?? Problema Resuelto

**Problema**: Al ejecutar la aplicación (darle "Play"), la base de datos `restaurante.db` se estaba recreando o intentando sobrescribir con datos semilla, perdiendo todos los platillos y configuraciones reales.

**Solución**: Se desactivó la creación automática de la base de datos y se comentaron los datos semilla para que la aplicación use SIEMPRE la base de datos existente con todos sus datos.

---

## ? Cambios Realizados

### 1. **MainForm.cs** - Línea 18
**ANTES:**
```csharp
public MainForm()
{
    InitializeComponent();
    db = new RestauranteContext();
    db.Database.EnsureCreated(); // ? Esta línea recreaba la BD
    CargarCategorias();
    // ...
}
```

**DESPUÉS:**
```csharp
public MainForm()
{
    InitializeComponent();
    db = new RestauranteContext();
    // db.Database.EnsureCreated(); // ? COMENTADO: La BD ya existe con datos reales
    CargarCategorias();
    // ...
}
```

**Qué hace**: Ya no intenta crear la base de datos automáticamente. Simplemente usa la que ya existe.

---

### 2. **RestauranteContext.cs** - Método OnModelCreating
**ANTES:**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // ... configuraciones de relaciones ...
    
    // Datos iniciales
    modelBuilder.Entity<Configuracion>().HasData(
        new Configuracion { Id = 1 }
    );
    
    modelBuilder.Entity<Categoria>().HasData(
        // 5 categorías de ejemplo
    );
    
    modelBuilder.Entity<Platillo>().HasData(
        // 6 platillos de ejemplo
    );
    
    // 13 mesas + 1 para llevar
}
```

**DESPUÉS:**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // ... configuraciones de relaciones ...
    
    // ? DATOS SEMILLA COMENTADOS: La BD real ya contiene todos los datos
    
    /*
    // Datos iniciales COMENTADOS
    modelBuilder.Entity<Configuracion>().HasData(/* ... */);
    modelBuilder.Entity<Categoria>().HasData(/* ... */);
    modelBuilder.Entity<Platillo>().HasData(/* ... */);
    // ...
    */
}
```

**Qué hace**: Los datos semilla ya no se intentan insertar en la base de datos.

---

## ?? Resultado

? **Ahora cuando ejecutas la aplicación (darle Play)**:
1. Se conecta a `restaurante.db` (la que ya existe)
2. **NO** intenta recrearla
3. **NO** intenta sobrescribir datos
4. Carga directamente todos los platillos, categorías, mesas y configuración que ya están guardados

? **Tu base de datos con todos los platillos reales se mantiene intacta**

? **Cualquier cambio que hagas desde el Panel de Administración se guarda permanentemente**

---

## ?? Ubicación de la Base de Datos

La base de datos se encuentra en:
```
restaurante/bin/Debug/net6.0-windows/restaurante.db
```

**IMPORTANTE**: 
- Este archivo contiene TODOS los datos (platillos, categorías, mesas, configuración)
- **Haz respaldos regulares** de este archivo
- Para restaurar datos, solo reemplaza este archivo con una copia de respaldo

---

## ?? ¿Qué pasa si borro el archivo .db?

Si por alguna razón borras el archivo `restaurante.db`:

1. La aplicación **dará error** porque espera que exista
2. **NO se creará automáticamente** (esto es lo que queríamos)
3. Tendrás que:
   - Restaurar desde un respaldo, o
   - Descomentar temporalmente las líneas que comentamos
   - Ejecutar la app una vez para crear la BD con datos de ejemplo
   - Volver a comentar las líneas
   - Agregar tus platillos reales desde el Panel de Administración

---

## ?? Si Necesitas Crear una Base de Datos Desde Cero

En caso de que necesites crear una nueva base de datos limpia:

1. **Elimina** el archivo `restaurante.db` actual
2. **Descomenta** las líneas que comentamos:
   - En `MainForm.cs` línea 18: `db.Database.EnsureCreated();`
   - En `RestauranteContext.cs`: todos los `HasData()`
3. **Ejecuta** la aplicación una vez
4. **Vuelve a comentar** esas líneas
5. **Agrega** tus datos reales desde el Panel de Administración

---

## ?? Datos Actuales en tu Base de Datos

Según el archivo `restaurante.db` que tienes, contiene:

### Categorías (16 categorías):
- Camarones
- Filetes
- Postres
- Bebidas
- Cervezas
- Desayunos
- Cocteles
- Entradas
- Refrescos
- Otros Platillos
- Platillos Para Llevar
- Licores
- Vinos De Mesa
- Medias Ord Camaron
- Medias Ord Filetes
- Dulces

### Platillos: **Más de 150 platillos** con precios actualizados

### Configuración:
- Nombre: Mariscos Pulido
- Dirección: F. Villa #22 Buenavista, Jal.
- Teléfono: 3857333334

### Mesas: 14 mesas (Mesa 1-13 + Mesa "Para Llevar")

**¡Todos estos datos ahora están PROTEGIDOS!** ?

---

## ?? Importante: Migraciones de Entity Framework

Si en el futuro necesitas agregar columnas nuevas a las tablas o hacer cambios en la estructura:

1. **NO uses migraciones automáticas** sin respaldo
2. **Haz SIEMPRE un respaldo** de `restaurante.db` antes de cualquier cambio
3. Si usas comandos como `Add-Migration` o `Update-Database`, pueden sobrescribir datos

---

## ? Conclusión

Con estos cambios, tu base de datos está protegida y la aplicación:
- ? Usa siempre los datos reales
- ? No recrea la base de datos
- ? No sobrescribe platillos
- ? Mantiene todos los cambios que hagas
- ? Funciona igual cuando le das "Play" en Visual Studio o cuando ejecutas la versión portable

---

**Fecha de modificación**: 15 de diciembre de 2025  
**Versión del proyecto**: .NET 6  
**Estado**: Base de datos protegida ?
