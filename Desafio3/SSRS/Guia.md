# Guía de SSRS para Desafio3Recetas

## 1. Preparar SQL Server y SSRS

1. Instalar SQL Server Developer o Express y comprobar que la instancia esté iniciada.
2. Instalar la versión de **SQL Server Reporting Services** compatible con la versión de SQL Server. SSRS se instala como producto separado en versiones modernas.
3. Abrir **Report Server Configuration Manager** como administrador y conectarse a la instancia de SSRS.
4. En **Service Account**, conservar una cuenta administrada o una cuenta de servicio con permisos de lectura sobre la base de datos `Desafio3Recetas`.
5. En **Web Service URL**, aceptar el nombre virtual `ReportServer`, aplicar y probar una URL como `http://localhost/ReportServer`.
6. En **Database**, seleccionar **Change Database**, crear una base de datos nueva llamada `ReportServer` y finalizar el asistente.
7. En **Web Portal URL**, conservar el nombre virtual `Reports`, aplicar y comprobar `http://localhost/Reports`.
8. La API crea `Desafio3Recetas` al iniciar después de aplicar la migración. Si SSRS se configura antes, crear la base ejecutando la API una vez o aplicar la migración con `dotnet ef database update`.

## 2. Fuente de datos compartida o embebida

En Report Builder, crear una fuente de datos con proveedor **Microsoft SQL Server**:

```text
Data Source=localhost;Initial Catalog=Desafio3Recetas;Integrated Security=True;TrustServerCertificate=True
```

Para LocalDB, sustituir `localhost` por `(localdb)\\MSSQLLocalDB`. La cuenta utilizada por el servicio de SSRS debe tener permisos `db_datareader` en `Desafio3Recetas`.

## 3. Reporte 1: recetas e ingredientes

Nombre recomendado: `Reporte1_RecetasIngredientes`.

Dataset `dsRecetasIngredientes`:

```sql
SELECT
	r.Id AS RecetaId,
	r.Nombre AS Receta,
	r.Descripcion,
	r.TiempoPreparacion,
	i.Id AS IngredienteId,
	i.Nombre AS Ingrediente,
	i.Cantidad,
	i.UnidadMedida
FROM dbo.Recetas AS r
LEFT JOIN dbo.Ingredientes AS i ON i.RecetaId = r.Id
WHERE (@MaxTiempo IS NULL OR r.TiempoPreparacion <= @MaxTiempo)
ORDER BY r.Nombre, i.Id;
```

En Report Builder:

- Crear un parámetro de dataset llamado `@MaxTiempo`, de tipo `Int32`.
- Crear el parámetro de informe `MaxTiempo`, tipo Integer, permitir valor `NULL` y asociarlo al parámetro `@MaxTiempo`.
- Insertar una tabla/matriz con grupo padre por `RecetaId` o `Receta`.
- En la fila de grupo mostrar `Receta`, `Descripcion` y `TiempoPreparacion`.
- En la fila de detalle mostrar `Ingrediente`, `Cantidad` y `UnidadMedida`.
- Ordenar el grupo por `Receta` y dejar el detalle por `IngredienteId`.
- Agregar un título como `Recetas con ingredientes` y una caja de texto que muestre el filtro aplicado.

La URL de prueba en el portal es similar a:

```text
http://localhost/ReportServer?/Desafio3Recetas/Reporte1_RecetasIngredientes&rs:Command=Render&MaxTiempo=30
```

## 4. Reporte 2: cantidad y detalle de pasos

Nombre recomendado: `Reporte2_PasosReceta`.

Dataset `dsPasosReceta`:

```sql
SELECT
	r.Id AS RecetaId,
	r.Nombre AS Receta,
	COUNT(p.Id) OVER (PARTITION BY r.Id) AS CantidadPasos,
	p.Orden,
	p.Descripcion AS Paso
FROM dbo.Recetas AS r
LEFT JOIN dbo.PasosPreparacion AS p ON p.RecetaId = r.Id
ORDER BY r.Nombre, p.Orden;
```

En Report Builder:

- Crear una tabla con un grupo padre por `RecetaId`.
- En la cabecera del grupo mostrar `Receta` y `CantidadPasos`.
- En la fila de detalle mostrar `Orden` y `Paso`.
- Ordenar el grupo por `Receta` y el detalle por `Orden` ascendente.
- Mantener el `LEFT JOIN` para que también aparezcan recetas sin pasos, con cantidad cero.
- Agregar un título como `Pasos de preparación por receta`.

La URL de prueba es similar a:

```text
http://localhost/ReportServer?/Desafio3Recetas/Reporte2_PasosReceta&rs:Command=Render
```

## 5. Publicar

1. En Report Builder seleccionar **File > Save As**.
2. Seleccionar el servidor `http://localhost/ReportServer`.
3. Crear la carpeta `/Desafio3Recetas` desde el portal si no existe.
4. Guardar cada informe dentro de esa carpeta con los nombres indicados.
5. Abrir cada informe desde `http://localhost/Reports`, probar el parámetro y verificar que la cuenta de SSRS pueda leer `Desafio3Recetas`.

## 6. Embeber en ASP.NET Core

La página `Pages/Reports/Index.cshtml` usa iframes y la URL `rs:Command=Render`. Se accede en `/Reports`; el `[Authorize]` de su PageModel exige iniciar sesión en la API.

La autenticación del iframe contra SSRS es independiente de la cookie de la API. En un entorno local puede funcionar con autenticación Windows si el navegador y SSRS están en el mismo dominio. En producción se debe configurar autenticación integrada/proxy/reverse proxy y no habilitar acceso anónimo sin proteger los informes.
