# Prueba de Integración para Simulación de Torneo

## Índice

- [Prueba de Integración para Simulación de Torneo](#prueba-de-integración-para-simulación-de-torneo)
  - [Índice](#índice)
  - [Introducción](#introducción)
  - [Objetivo de la Prueba](#objetivo-de-la-prueba)
  - [Configuración Inicial y Entorno](#configuración-inicial-y-entorno)
    - [Manejo de Rutas de Logs](#manejo-de-rutas-de-logs)
  - [Fases de la Prueba (`SimulateTournament_ShouldLogEventsAndReturnResult`)](#fases-de-la-prueba-simulatetournament_shouldlogeventsandreturnresult)
    - [Arrange (Preparación)](#arrange-preparación)
    - [Act (Acción)](#act-acción)
    - [Assert (Verificación)](#assert-verificación)
  - [Registro de Eventos (Logging)](#registro-de-eventos-logging)
  - [Uso de Configuración Externa para Datos de Prueba (`TournamentTestConfig.json`)](#uso-de-configuración-externa-para-datos-de-prueba-tournamenttestconfigjson)
  - [Conclusión](#conclusión)

---

## Introducción

Este documento describe la prueba de integración implementada en `SimulateTournamentTests.cs`, ubicada en el proyecto `TennisTournament.Tests.Integration` dentro del subdirectorio `Features`. Esta prueba se enfoca en validar la funcionalidad completa de simulación de un torneo, aprovechando la arquitectura limpia de la solución.

## Objetivo de la Prueba

El objetivo principal de `SimulateTournamentTests` es asegurar que el endpoint de la API para simular un torneo (`/api/tournaments/{tournamentId}/simulate`) funcione correctamente. Esto incluye:

1.  La correcta inicialización de datos necesarios para el torneo.
2.  La ejecución de la lógica de simulación del torneo por parte de la API.
3.  La recepción de una respuesta HTTP exitosa (OK).
4.  La validación del resultado de la simulación (por ejemplo, la obtención de un ganador).
5.  El registro de los eventos clave durante la ejecución de la prueba en un archivo de log.

## Configuración Inicial y Entorno

La clase de prueba utiliza `IClassFixture<CustomWebApplicationFactory>` para configurar un entorno de prueba con una instancia de la aplicación web. Esto permite realizar solicitudes HTTP reales a los endpoints de la API.

### Manejo de Rutas de Logs

Una característica importante de esta prueba es su capacidad para adaptar la ruta de guardado de los archivos de log, dependiendo de si se ejecuta en un entorno local (durante `dotnet test`) o dentro de un contenedor Docker.

- Se detecta el entorno (Docker o local, por ejemplo, mediante una variable de entorno como `DOTNET_RUNNING_IN_CONTAINER`).
- Si es Docker, se utiliza una ruta predefinida dentro del contenedor (ej. `/app/Logs`).
- Si es local, se calcula una ruta relativa al directorio raíz del proyecto de pruebas.
- Se asegura que el directorio de logs exista antes de escribir en él.

Esto garantiza que los logs se generen y se puedan acceder independientemente del entorno de ejecución.

## Fases de la Prueba (`SimulateTournament_ShouldLogEventsAndReturnResult`)

La prueba principal sigue el patrón Arrange-Act-Assert:

### Arrange (Preparación)

- Se obtiene o crea un ID de torneo válido utilizando `SeedDataExtensions.GetOrCreateTournamentIdAsync`. Esto asegura que exista un torneo en la base de datos sobre el cual operar.
- Se inicializa una lista para recolectar las líneas de log que se generarán durante la prueba.

### Act (Acción)

- Se realiza una solicitud HTTP POST al endpoint `/api/tournaments/{tournamentId}/simulate` sin cuerpo de solicitud, ya que la simulación se basa en el ID del torneo.
- Se registra el estado de esta solicitud en las líneas de log.

### Assert (Verificación)

- Se verifica que el código de estado de la respuesta HTTP sea `OK` (200).
- Se deserializa el contenido de la respuesta para obtener un `ResultDto`.
- Se verifica que el resultado no sea nulo y se registra la información del ganador en las líneas de log.

## Registro de Eventos (Logging)

Al finalizar la fase de Assert, todas las líneas de log recolectadas (inicio de la prueba, envío de la solicitud, estado de la respuesta, resultado obtenido) se guardan en el archivo `SimulateTournamentTest.log` en la ruta previamente determinada (local o Docker). Esto facilita la depuración y el seguimiento del comportamiento de la prueba.

## Uso de Configuración Externa para Datos de Prueba (`TournamentTestConfig.json`)

Aunque no se utiliza directamente en el fragmento de código de `SimulateTournamentTests.cs` proporcionado para la simulación específica, la existencia de un archivo como `Config/TournamentTestConfig.json` en el proyecto `TennisTournament.Tests.Integration` sugiere una estrategia para externalizar la definición de los datos de prueba.

Este archivo JSON permite definir:

- Si se deben sembrar torneos (`EnableTournamentSeed`).
- Una lista de torneos con sus propiedades (ID, tipo, fechas, estado) y una lista de jugadores asociados a cada torneo, incluyendo sus atributos (ID, nombre, nivel de habilidad, tipo).

**Propósito y Ventajas:**

- **Centralización de Datos de Prueba:** Permite definir un conjunto de datos base de forma clara y estructurada fuera del código C#.
- **Facilidad de Mantenimiento:** Modificar los datos de prueba (añadir jugadores, cambiar fechas de torneos) se puede hacer editando el JSON sin recompilar el código de las pruebas.
- **Reutilización:** Estos datos podrían ser cargados por `SeedData` o clases de utilidad para poblar la base de datos antes de ejecutar diferentes conjuntos de pruebas de integración.
- **Escenarios Complejos:** Facilita la creación de escenarios de prueba más complejos con múltiples entidades y relaciones.

La lógica de `SeedDataExtensions` o clases similares en el directorio `Data` podrían leer este archivo de configuración para poblar la base de datos de manera consistente antes de que las pruebas (como `SimulateTournamentTests`) se ejecuten.

## Conclusión

La prueba `SimulateTournamentTests.cs` valida de manera integral la funcionalidad de simulación de torneos, desde la preparación de datos hasta la verificación del resultado y el registro de eventos. La adaptación de rutas de log y la potencial integración con archivos de configuración JSON para el sembrado de datos demuestran un enfoque robusto y flexible para las pruebas de integración en la arquitectura limpia de la aplicación.

---

_Este documento detalla la prueba de integración para la simulación de torneos y el uso de configuración externa para datos de prueba._
