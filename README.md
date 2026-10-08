# Desafio3 – API de Recetas

API REST en ASP.NET Core (.NET 10) para gestionar recetas, ingredientes y pasos
de preparación, con autenticación y autorización mediante ASP.NET Core Identity
y reportes en SQL Server Reporting Services (SSRS).

## Integrantes

| Nombre | Carnet |
|---|---|
| Sebastián Hugo Acosta Rosales | AR201154 |
| Sofía Elizabeth Recinos Molina | RM220242 |
| Douglas Enrique Vanegas Montenegro | VM220243 |

## Tecnologías

- .NET 10 / ASP.NET Core Web API + Razor Pages
- Entity Framework Core 10 + SQL Server (LocalDB)
- ASP.NET Core Identity (usuarios, roles, autenticación por cookie)
- Scalar (documentación OpenAPI, solo en Development)
- SQL Server Reporting Services (SSRS)

## Requisitos previos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (incluido con Visual Studio) o una instancia de SQL Server
- (Opcional) SSRS y Report Builder para los reportes

## Configuración

1. Clonar el repositorio:
```bash
   git clone https://github.com/dougxanny05/Desafio3.git
   cd Desafio3
```

2. Revisar la cadena de conexión en `Desafio3/appsettings.json`
   (`ConnectionStrings:DefaultConnection`).

3. **Configurar la contraseña del administrador inicial** con User Secrets:
```bash
   dotnet user-secrets set "IdentitySeed:AdminPassword" "TU_CONTRASEÑA" --project Desafio3/Desafio3.csproj
```

   **Uso de este comando**
   - Ejecutarlo desde la raíz de la solución (donde está `Desafio3.slnx`).
   - Guarda la contraseña del administrador fuera del repositorio. El correo
     del administrador se define en `appsettings.json` (`IdentitySeed:AdminEmail`,
     por defecto `admin@desafio3.local`).
   - La aplicación la lee al iniciar por primera vez y crea el usuario
     administrador. Si la contraseña no está configurada y el administrador
     no existe, la aplicación no arranca.
   - La contraseña debe cumplir la política: mínimo 8 caracteres, con
     mayúscula, minúscula, número y símbolo.
   - Solo se usa al crear al administrador; cambiarla después no modifica
     una cuenta ya existente.
   - User Secrets solo aplica en entorno `Development`. En producción usar la
     variable de entorno `IdentitySeed__AdminPassword`.

4. Ejecutar la aplicación (crea la base de datos, los roles y el administrador
   automáticamente):
```bash
   dotnet run --project Desafio3/Desafio3.csproj
```

5. Abrir la documentación interactiva en `http://localhost:5031/scalar/v1`.

## Autenticación y autorización

| Rol | Permisos |
|---|---|
| Administrador | Consultar, crear, modificar y eliminar |
| Usuario | Solo consultar (rol asignado automáticamente al registrarse) |

Flujo básico:
1. `POST /register` con `{ "email": "...", "password": "..." }`
2. `POST /login?useCookies=true` con las mismas credenciales
3. Usar la cookie de sesión en las siguientes peticiones

## Endpoints principales

| Recurso | GET | POST / PUT / DELETE |
|---|---|---|
| `/api/recetas` | Autenticado | Administrador |
| `/api/ingredientes` | Autenticado | Administrador |
| `/api/pasospreparacion` | Autenticado | Administrador |
| `/Reports` (página Razor) | Autenticado | – |

## Estructura del proyecto

```
Desafio3/
├── Controllers/   API REST
├── Models/        Entidades
├── DTOs/          Contratos de entrada y salida
├── Data/          DbContext (Identity + negocio)
├── Identity/      UsuarioManager y roles
├── Migrations/    Migraciones de EF Core
├── Pages/Reports/ Página con reportes SSRS
├── SSRS/          Guía de reportes
└── Validation/    Validaciones personalizadas
```

## Guia para Reportes SSRS

Ver [`Desafio3/SSRS/Guia.md`](Desafio3/SSRS/Guia.md).

## Licencia

MIT. Ver `Desafio3/LICENSE.txt`.
