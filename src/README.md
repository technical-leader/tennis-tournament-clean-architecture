## Pasos iniciales para el uso de la solución.

**Consideraciones**:

- Se espera que quien pruebe esta solución tenga conocimientos sobre desarrollo en .Net Core. La solución esta desarrollada en .Net 9 C#.
- Se facilita la cadena de conexión a la base de datos de ejemplo en `TennisTournament.API\appsettings.example.json` (dejar en el directorio el archivo `appsettings.json` actualizado) para utilizar un SQL Server.
- El usuario debe estar familizarizado con el desarrollo en .Net Core para probar la solución con la UI de Swagger. Mediante esta página http://localhost:5000/index.html podrá acceder una vez la solución esté corriendo. En esta página podrá ver los endpoint disponibles de la solución.
- Históricamente, las aplicaciones .Net Core atendían en los port: 5000 (http) y 5001 (https). Si se requiere, puede definirse el port a utilizar mediante la edición: `TennisTournament.API\appsettings.json`

### Luego de desplegar la solución en su entorno:

1. El directorio de la solución es el que contiene al archivo: `TennisTournament.sln`
2. La estructura de los directorios de la solución es:

```
  📁 src/
     ├── 📁 TennisTournament.Domain/              # Modelado de entidades y lógica de negocio
     ├── 📁 TennisTournament.Application/         # Servicios y reglas de aplicación
     ├── 📁 TennisTournament.Infrastructure/      # Persistencia y adaptadores de datos
     ├── 📁 TennisTournament.API/                 # Exposición vía REST API
     ├── 📁 TennisTournament.Tests.Unit/          # Pruebas unitarias
     ├── 📁 TennisTournament.Tests.Integration/   # Pruebas de integración
     ├── 📄 Dockerfile                            # Alta de imagen Docker
     ├── 📄 README.md                             # Documentación principal
     ├── 📄 TennisTournament.sln                  # Solución de .NET 9 C#
```

3. Desde el directorio de la solución se puede ejecutar `Visual Studio Code` mediante

```pwsh
code .
```

4. Con el IDE puede acceder al intérprete de comandos mediante CTRL + ñ
5. Abra el arhivo: `appsettings.json` que está en el directorio `TennisTournament.API`
6. Modifique los datos relacionados a su entorno. Considere que debe usar por ejemplo SQL Server 2022 (developer). Los datos a modificar están indicados mediante { ... }
7. Los comandos básicos para crear la solución en modo desarrollo son:
   1. Desde el directorio de la solución en la `TERMINAL` ingrese:
      ```pwsh
        dotnet build
      ```
   2. Para gestionar la persistencia de los datos:
      1. Generar una migración inicial: Ejecuta en la raíz del proyecto:
      ```pwsh
        dotnet ef migrations add InitialCreate --project TennisTournament.Infrastructure --startup-project TennisTournament.API
      ```
      2. Esto creará la carpeta Migrations y los archivos necesarios.
      ```pwsh
        dotnet ef database update --project TennisTournament.Infrastructure --startup-project TennisTournament.API
      ```
      3. Esto eliminará la base de datos creada (cuidado, perderá los datos).
      ```pwsh
        dotnet ef database drop --project TennisTournament.Infrastructure --startup-project TennisTournament.API -f
      ```
8. Para ejecutar la aplicación:

```pwsh
  dotnet run --project TennisTournament.API
```

9.  Al estar corriendo la aplicación puede interactuar con ella mediantes sus endpoint, se facilita Swagger para ello (http://localhost:5000/index.html).
10. A modo de prueba, en esta instancia tiene información en la base de datos. Se trata de dos ejemplos de torneos, uno masculino y otro femenino. Cada torneo tiene cuatro participantes. Puede desde Swagger consultar por los torneos como también puede simular uno para luego consultar la información generada.
