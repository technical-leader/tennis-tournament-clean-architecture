# Propuesta - Simulación de Torneos de Tenis con Eliminación Directa

## Índice

- [Propuesta - Simulación de Torneos de Tenis con Eliminación Directa](#propuesta---simulación-de-torneos-de-tenis-con-eliminación-directa)
  - [Índice](#índice)
  - [1. Requerimientos del Sistema](#1-requerimientos-del-sistema)
    - [1.1 Modalidad y Dinámica del Torneo](#11-modalidad-y-dinámica-del-torneo)
    - [1.2 Requerimientos de Simulación y Resultado](#12-requerimientos-de-simulación-y-resultado)
  - [2. Modelo de Datos y Estructura en Base de Datos](#2-modelo-de-datos-y-estructura-en-base-de-datos)
    - [2.1 Tablas y Campos Principales](#21-tablas-y-campos-principales)
    - [2.2 Ejemplo de Representación de un Torneo](#22-ejemplo-de-representación-de-un-torneo)
  - [3. Solución y Lógica de Simulación](#3-solución-y-lógica-de-simulación)
    - [3.1 Estructura General de la Solución (Arquitectura Limpia)](#31-estructura-general-de-la-solución-arquitectura-limpia)
    - [3.2 Detalle del Algoritmo de Simulación de Rondas](#32-detalle-del-algoritmo-de-simulación-de-rondas)
    - [3.3 Ejecución en Paralelo](#33-ejecución-en-paralelo)
  - [4. Integración con Endpoints (Interacción API)](#4-integración-con-endpoints-interacción-api)
    - [Endpoints Actuales](#endpoints-actuales)
    - [Posibles Extensiones](#posibles-extensiones)
  - [5. Ciclo de Vida de la Solución](#5-ciclo-de-vida-de-la-solución)
  - [6. Consideraciones Finales](#6-consideraciones-finales)

---

A continuación, presento un documento con los requerimientos y la propuesta de solución para la simulación de torneos de tenis con eliminación directa. Este documento detalla la lógica y la arquitectura a seguir.

## 1. Requerimientos del Sistema

### 1.1 Modalidad y Dinámica del Torneo

- **Eliminación Directa**:

  - El torneo se organiza en rondas eliminatorias. En cada ronda, se emparejan los jugadores; el perdedor de cada enfrentamiento es eliminado y el ganador avanza a la siguiente ronda.
  - Se asume que la cantidad de jugadores es una potencia de 2.

- **Tipos de Torneo y Jugadores**:

  - **Torneo Masculino**:
    - Los jugadores tienen, además del nombre y nivel de habilidad (0 a 100), parámetros adicionales: fuerza y velocidad.
  - **Torneo Femenino**:
    - Las jugadoras cuentan, además del nombre y nivel de habilidad, con el parámetro de tiempo de reacción.

- **Enfrentamientos (Matches)**:
  - Cada partido se resuelve combinando el nivel de habilidad con un factor aleatorio (representando la “suerte”).
  - En el torneo masculino se integran, además, la fuerza y la velocidad. En el femenino, se utiliza el tiempo de reacción (o una función inversa de éste para penalizar tiempos más altos).
  - No existen empates, por lo que en cada enfrentamiento se determina un ganador.

### 1.2 Requerimientos de Simulación y Resultado

- **Objetivo**:

  - Dado un grupo de jugadores, se simula el torneo completo y se obtiene como salida el ganador final.
  - Se deben registrar todos los partidos (matches) y, finalmente, consolidar un resultado que incluya al campeón y los detalles de cada ronda.

- **Ciclo de Rondas**:

  - **Primera Ronda**: Se forman los pares a partir de una lista (posiblemente barajada) de jugadores. Por ejemplo, con 4 jugadores, se crean dos partidos.
  - **Ronda Siguiente**: Los ganadores de la ronda anterior se colocan en una nueva lista, se emparejan nuevamente y se simulan los partidos.
  - **Final**: El proceso se repite hasta que solo quede un jugador, quien se declara campeón.

- **Algoritmo de Simulación de Rondas**:
  - Dada una lista de jugadores:
    1. **Emparejamiento**: Tomar la lista en orden y formar pares consecutivos.
    2. **Simulación del Partido**: Para cada par, utilizar una estrategia que combine el nivel de habilidad, un factor aleatorio (suerte) y, según el tipo de torneo, los atributos adicionales (fuerza/velocidad para masculinos o tiempo de reacción para femeninos).
    3. **Selección del Ganador**: El jugador con el puntaje mayor gana el partido.
    4. **Actualización de la Lista**: Los ganadores son almacenados en una nueva lista.
    5. **Iteración**: Repetir el proceso con la nueva lista hasta que solo quede un jugador.
  - La lógica puede implementarse de forma recursiva o mediante un ciclo iterativo. Además, se considera el paralelismo en la simulación de partidos dentro de cada ronda para optimizar el rendimiento en el sistema.

## 2. Modelo de Datos y Estructura en Base de Datos

### 2.1 Tablas y Campos Principales

- **Players**
  - Campos: `Id`, `Name`, `SkillLevel`, `PlayerType` (indica masculino o femenino), `TournamentId`, junto a atributos específicos (para femeninos: `ReactionTime`; para masculinos: `Strength` y `Speed`).
- **Matches**
  - Campos: `Id`, `Player1Id`, `Player2Id`, `WinnerId`, `Round`, `MatchDate`, `TournamentId` y, opcionalmente, referencia a un `ResultId` si se asocia al resultado global.
- **Tournaments**
  - Campos: `Id`, `Type` (Male/Female), `StartDate`, `EndDate`, `Status` (por ejemplo: Scheduled, Ongoing, Completed).
- **Results**
  - Campos: `Id`, `TournamentId`, `TournamentType`, `WinnerId`, `WinnerDisplay`, `CompletionDate`, y se puede incluir la colección de partidos jugados (los _Matches_).

### 2.2 Ejemplo de Representación de un Torneo

La representación de datos para un torneo incluye:

- Datos generales: `id`, `type` (Male/Female) y sus visualizaciones.
- Listas de `playerIds` y `playerNames`.
- Fechas de inicio y fin.
- Estado del torneo.
- Un arreglo de matches (inicialmente vacío, que se llenará conforme se simula el torneo).

Cada _Match_ se modela con atributos como el identificador del partido, los identificadores de los jugadores, el identificador del ganador, los nombres de los jugadores y del ganador para visualización, la ronda y la fecha del partido.

## 3. Solución y Lógica de Simulación

### 3.1 Estructura General de la Solución (Arquitectura Limpia)

- **Capa de Dominio**:

  - Entidades principales: `Player` (que puede ser abstracta), `MalePlayer`, `FemalePlayer`, `Tournament`, `Match` y `TournamentResult`.
  - Interfaz para la estrategia de simulación (`IMatchSimulationStrategy`) con implementaciones específicas para torneos masculinos y femeninos.

- **Capa de Aplicación**:
  - Casos de uso o services, por ejemplo, un servicio llamado `TournamentSimulator` que:
    - Recibe un objeto `Tournament` con la lista de jugadores.
    - Valida que la cantidad de jugadores sea una potencia de 2.
    - Procede a iterar o llamar recursivamente a la función de simulación de rondas:
      - En cada ronda, tomando la lista de jugadores actual, se forma una lista de partidos (pares generados a partir del orden de la lista), se simulan los partidos usando la estrategia y se obtiene la lista de ganadores.
      - Se guarda la información de cada partido en una entidad `Match` con su ronda correspondiente y la fecha del encuentro.
    - Al finalizar la simulación (cuando la lista de ganadores tiene un solo jugador), se construye un objeto `TournamentResult` integrando el ganador y la colección completa de partidos.
- **Capa de Infraestructura**:
  - Integración con la base de datos para la persistencia de `Players`, `Matches`, `Tournaments` y `Results`.
  - Implementación de repositorios y unidades de trabajo (UoW) para persistir y recuperar la información, cumpliendo los principios SOLID en el sistema.

### 3.2 Detalle del Algoritmo de Simulación de Rondas

**Algoritmo (Lógica de Negocio)**:

1. **Inicialización**:
   - Verificar que la lista de jugadores tenga un tamaño que sea una potencia de 2.
   - (Opcional) Barajar o validar el orden de la lista.
2. **Ciclo para cada ronda**:
   - Para cada ronda, iterar sobre la lista de jugadores en pasos de 2.
   - Para cada par, invocar el método de simulación del partido:
     - Obtener el "score" de cada jugador usando:
       - Para **masculinos**: base calculado a partir del `SkillLevel` + (ponderación de `Strength` y `Speed`) + un valor aleatorio.
       - Para **femeninas**: base calculado a partir del `SkillLevel` + (transformación del `ReactionTime`) + un valor aleatorio.
     - Determinar el ganador (el que tenga el score mayor).
     - Generar una instancia de `Match` con todos los datos (IDs, nombres, puntajes, ronda, fecha del partido, etc.).
   - Reemplazar la lista actual de jugadores con la lista de ganadores obtenidos en esta ronda.
   - Incrementar el contador de ronda.
3. **Finalización**:
   - Cuando solo quede un jugador en la lista, asignarlo como campeón del torneo.
   - Consolidar el objeto `TournamentResult` con:
     - La identificación del torneo,
     - El ganador final,
     - La fecha de finalización,
     - Y la colección completa de `Matches` registradas a lo largo de la simulación.

### 3.3 Ejecución en Paralelo

- Se puede optimizar el rendimiento en el sistema ejecutando cada par de partidos en paralelo (por ejemplo, mediante tareas asíncronas o paralelismo en el nivel de la ronda), dado que los partidos dentro de una misma ronda son independientes.

---

## 4. Integración con Endpoints (Interacción API)

### Endpoints Actuales

- **Matches**:
  - GET /api/Matches
  - GET /api/Matches/{id}
  - GET /api/Matches/bytournament/{tournamentId}
  - GET /api/Matches/byplayer/{playerId}
- **Players**:
  - GET /api/Players
  - GET /api/Players/{id}
  - DELETE /api/Players/{id}
  - POST /api/Players/male
  - POST /api/Players/female
  - PUT /api/Players/male/{id}
  - PUT /api/Players/female/{id}
- **Results**:
  - GET /api/Results
  - GET /api/Results/{id}
  - GET /api/Results/bytype/{type}
  - GET /api/Results/bydate
  - GET /api/Results/bywinner/{playerId}
- **Tournaments**:
  - GET /api/Tournaments
  - POST /api/Tournaments
  - GET /api/Tournaments/{id}
  - GET /api/Tournaments/bydate
  - POST /api/Tournaments/{id}/simulate
  - GET /api/Tournaments/{id}/result

### Posibles Extensiones

- Se pueden agregar endpoints extra para re-simular torneos, actualizar estados o consultar estadísticas específicas (por ronda, por jugador, etc.).
- El endpoint **POST /api/Tournaments/{id}/simulate** se encarga de invocar el servicio de simulación que utiliza el algoritmo descrito para generar los partidos y determinar al ganador final.

---

## 5. Ciclo de Vida de la Solución

1. **Configuración Inicial**:
   - El usuario registra los jugadores (masculinos y femeninos) con sus atributos.
   - Se crea un torneo especificando el tipo (Male o Female) y se asocian los jugadores al torneo.
2. **Simulación**:

   - Al llamar al endpoint de simulación de torneos, la lógica del servicio verifica la integridad de los datos (cantidad de jugadores potencia de 2).
   - Se inicia el proceso de simulación de rondas:
     - Emparejamiento de jugadores.
     - Simulación del partido (aplicando la estrategia y generando un score).
     - Registro de cada partida y actualización de la lista de ganadores.
   - Se continúa hasta que se determine un único campeón.

3. **Persistencia y Consulta**:
   - Todos los partidos (Matches) y el resultado final (TournamentResult) se almacenan en la base de datos.
   - Los endpoints del sistema permiten consultar tanto el detalle de cada enfrentamiento, como el resultado final y la evolución del torneo.

---

## 6. Consideraciones Finales

- **Buenas Prácticas y OOP**:
  - Se enfatiza en el correcto uso de la Programación Orientada a Objetos en el sistema, implementando una correcta separación de responsabilidades a través de la Clean Architecture.
  - Se utilizan patrones de diseño, como el Strategy para la simulación de partidos, permitiendo una solución extensible y de mantenimiento sencillo.
- **Ejecución y Paralelismo**:
  - La lógica de simulación de rondas en el sistema puede ejecutarse de forma secuencial o paralela en cada ronda, optimizando el rendimiento en torneos con gran cantidad de jugadores.
- **Versionado y Documentación**:
  - Se recomienda utilizar un sistema de versionado para gestionar la solución, y documentar cada módulo y endpoint para facilitar futuras extensiones en el sistema.

---

Este documento integrador recoge la totalidad de los requerimientos y la lógica de la solución a implementar en el sistema. Se puede utilizar esta especificación para transformar la lógica descrita en código, definiendo las entidades, servicios (como el simulador de torneos), estrategias de simulación y la integración de API según los endpoints expuestos. Si se requieren aclaraciones adicionales o ajustes en la lógica, se pueden documentar como notas complementarias.
