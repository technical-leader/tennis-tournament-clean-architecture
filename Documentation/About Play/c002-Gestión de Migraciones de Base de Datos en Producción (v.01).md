## Gestión de Migraciones de Base de Datos en Producción

## Índice

- [Gestión de Migraciones de Base de Datos en Producción](#gestión-de-migraciones-de-base-de-datos-en-producción)
- [Índice](#índice)
- [Introducción](#introducción)
- [Principios Clave para Producción](#principios-clave-para-producción)
- [Generación de Scripts de Migración](#generación-de-scripts-de-migración)
  - [Método 1: Usando Docker](#método-1-usando-docker)
    - [Comando Genérico con Docker](#comando-genérico-con-docker)

---

## Introducción

En un entorno de producción, la base de datos es un recurso crítico que generalmente ya existe y contiene datos importantes. Por lo tanto, a mi entender, no se deben aplicar las migraciones de Entity Framework Core automáticamente al iniciar la aplicación.

## Principios Clave para Producción

La lógica para aplicar migraciones automáticamente (por ejemplo, mediante `dbContext.Database.Migrate()`) en la aplicación debe estar condicionada para ejecutarse **únicamente en entornos de desarrollo**. Esto se logra, por ejemplo, envolviendo la llamada dentro de una verificación del entorno (como `if (app.Environment.IsDevelopment())`) en el archivo de inicio de la aplicación (ej. `Program.cs`).

Para aplicar cambios en el esquema de la base de datos de producción, la práctica recomendada es generar scripts SQL a partir de las migraciones existentes. Estos scripts pueden ser revisados y aplicados de forma controlada por un administrador de base de datos o como parte de un proceso de despliegue automatizado (CI/CD).

## Generación de Scripts de Migración

El comando para generar un script de migración se puede obtener principalmente de dos formas:

### Método 1: Usando Docker

Este método implica montar el código fuente en un contenedor que tenga el SDK de .NET.

#### Comando Genérico con Docker

```bash
docker run --rm \
  -v "C:\Ruta\Completa\Al\Directorio\De\La\Solución:/app" \
  -w /app/NombreProyectoAPI \
  -e "ConnectionStrings__DefaultConnection=CadenaDeConexiónAProducción" \
  mcr.microsoft.com/dotnet/sdk:9.0 \
  dotnet ef migrations script --project ../NombreProyectoInfraestructura --startup-project . -o /app/ruta/donde/guardar/el/script.sql --idempotent
```

_(Nota: Reemplazar las rutas y nombres de proyecto con los valores correctos del proyecto.)_

- Para mi entorno

```pwsh
docker run --rm `
  -v "C:\Desarrollo\Desa2024\MicroServicios\GeoPagos\Challenge Técnico:/app" `
  -w /app/TennisTournament.API `
  -e "ConnectionStrings__DefaultConnection=Data Source=host.docker.internal,1563;Initial Catalog=TennisTournamentDb;User ID=sa;Password=Belgaum135;Connect Timeout=15;Trust Server Certificate=True;Authentication=SqlPassword;Application Name=TennisTournament;Command Timeout=30" `
  mcr.microsoft.com/dotnet/sdk:9.0 `
  sh -c 'dotnet tool install --global dotnet-ef && export PATH="$PATH:/root/.dotnet/tools" && dotnet ef migrations script --project ../TennisTournament.Infrastructure --startup-project . -o /app/Documentation/Container/migration_script.sql --idempotent'
```

- Para consultar su contenido, puedo hacer lo siguiente directamente desde el PowerShell o usando un editor de texto:

  - Usando PowerShell para mostrar el contenido en la terminal: Abro PowerShell y ejecuto:

  ```powershell
  Get-Content "Documentation\Container\migration_script.sql"
  ```

  - Si el archivo es muy largo, puedo paginarlo:

  ```powershell
  Get-Content "Documentation\Container\migration_script.sql" | More
  ```

2. Desde el entorno de desarrallo local (bajo el SDK de .NET) hacer:
   - Correr la emigración para generar el script según la configuración actual de la DB de desarrollo.

```pwsh
dotnet ef migrations script --project TennisTournament.Infrastructure --startup-project TennisTournament.API -o Work\Scripts_T-SQL\#02-script.sql --idempotent
```

_Considerar que esta última es la solución que apliqué en mi entorno y esta sujeta a aceptación a las prácticas de la organización_
