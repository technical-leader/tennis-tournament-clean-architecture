# Pruebas Unitarias para Validación de Género en Torneos

## Índice

- [Pruebas Unitarias para Validación de Género en Torneos](#pruebas-unitarias-para-validación-de-género-en-torneos)
  - [Índice](#índice)
  - [1. Introducción](#1-introducción)
  - [2. Funcionalidad Bajo Prueba](#2-funcionalidad-bajo-prueba)
    - [2.1. Regla de Negocio](#21-regla-de-negocio)
    - [2.2. Componente Afectado](#22-componente-afectado)
  - [3. Estrategia de Pruebas Unitarias Aplicada](#3-estrategia-de-pruebas-unitarias-aplicada)
    - [3.1. Enfoque](#31-enfoque)
    - [3.2. Ubicación de las Pruebas](#32-ubicación-de-las-pruebas)
    - [3.3. Metodología](#33-metodología)
    - [3.4. Herramientas](#34-herramientas)
  - [4. Casos de Prueba Implementados](#4-casos-de-prueba-implementados)
    - [4.1. Pruebas del Constructor de `Tournament`](#41-pruebas-del-constructor-de-tournament)
    - [4.2. Pruebas del Método `AddPlayer` de `Tournament`](#42-pruebas-del-método-addplayer-de-tournament)
  - [5. Validaciones Clave Verificadas](#5-validaciones-clave-verificadas)
  - [6. Consideraciones Adicionales](#6-consideraciones-adicionales)
    - [6.1. Aislamiento](#61-aislamiento)
    - [6.2. Datos de Prueba](#62-datos-de-prueba)
  - [7. Conclusión](#7-conclusión)

---

## 1. Introducción

Este documento detalla la implementación de pruebas unitarias para una nueva funcionalidad de control en el sistema TennisTournament. El objetivo de esta funcionalidad es asegurar la consistencia de género entre un torneo y sus participantes: si un torneo es de tipo masculino, todos sus jugadores deben ser masculinos, y viceversa.

Las pruebas unitarias se han diseñado para verificar esta lógica de negocio directamente en la capa de dominio, garantizando su correcto funcionamiento de forma aislada.

## 2. Funcionalidad Bajo Prueba

### 2.1. Regla de Negocio

La regla de negocio principal es: "Para un torneo, todos los integrantes deben ser del mismo tipo (género) que establece el torneo".

Esto implica que:

- No se puede crear un torneo con una lista de jugadores que incluya géneros mixtos o un género que no coincida con el tipo de torneo.
- No se puede agregar un jugador a un torneo existente si el género del jugador no coincide con el tipo de torneo.

### 2.2. Componente Afectado

La lógica de esta validación reside principalmente en la entidad de dominio `TennisTournament.Domain.Entities.Tournament`. Específicamente, en:

- El constructor que acepta una lista inicial de jugadores: `Tournament(TournamentType type, List<Player> players)`.
- El método para agregar jugadores individualmente: `AddPlayer(Player player)`.
- El método para establecer una lista de jugadores: `SetPlayers(IEnumerable<Player> players)`.

## 3. Estrategia de Pruebas Unitarias Aplicada

### 3.1. Enfoque

Las pruebas se centran en la categoría "Pruebas de Servicios de Dominio" (o pruebas de entidades de dominio, en este caso), como se describe en el documento `tu01-Estrategia de Pruebas Unitarias (v.01).md`. El objetivo es probar la lógica de negocio de la entidad `Tournament` de forma aislada, sin dependencias de controladores, bases de datos u otros componentes de infraestructura.

### 3.2. Ubicación de las Pruebas

Las pruebas unitarias para esta funcionalidad se encuentran en el proyecto `TennisTournament.Tests.Unit`, específicamente en el archivo:
`c:\Desarrollo\Desa2024\MicroServicios\GeoPagos\Challenge Técnico\TennisTournament.Tests.Unit\Features\TournamentPlayerValidationTests.cs`

### 3.3. Metodología

Cada prueba sigue el patrón AAA (Arrange-Act-Assert):

1.  **Arrange**: Se preparan los objetos necesarios, como instancias de `MalePlayer`, `FemalePlayer` y listas de jugadores. Se define el tipo de torneo.
2.  **Act**: Se ejecuta la acción que se desea probar, como la creación de una instancia de `Tournament` o la llamada al método `AddPlayer`.
3.  **Assert**: Se verifica que el comportamiento sea el esperado. Esto incluye comprobar que se lance la excepción correcta (`ArgumentException`) en casos inválidos o que no se lance ninguna excepción y el estado del objeto sea el correcto en casos válidos.

### 3.4. Herramientas

- **xUnit**: Framework de pruebas utilizado para definir y ejecutar los tests.
- **Moq** y **FluentAssertions**: Aunque disponibles en el proyecto de tests, no son estrictamente necesarios para estos tests específicos de la entidad `Tournament`, ya que no se están mockeando dependencias complejas ni usando aserciones de FluentAssertions en el código proporcionado para `TournamentPlayerValidationTests.cs`. Las aserciones se realizan con `Assert` de xUnit.

## 4. Casos de Prueba Implementados

Se han implementado los siguientes casos de prueba en `TournamentPlayerValidationTests.cs`:

### 4.1. Pruebas del Constructor de `Tournament`

- `Constructor_WithMaleTournamentAndAllMalePlayers_ShouldSucceed`: Verifica que se pueda crear un torneo masculino con solo jugadores masculinos.
- `Constructor_WithMaleTournamentAndFemalePlayer_ShouldThrowArgumentException`: Verifica que se lance `ArgumentException` si se intenta crear un torneo masculino con una jugadora femenina.
- `Constructor_WithFemaleTournamentAndAllFemalePlayers_ShouldSucceed`: Verifica que se pueda crear un torneo femenino con solo jugadoras femeninas.
- `Constructor_WithFemaleTournamentAndMalePlayer_ShouldThrowArgumentException`: Verifica que se lance `ArgumentException` si se intenta crear un torneo femenino con un jugador masculino.

### 4.2. Pruebas del Método `AddPlayer` de `Tournament`

- `AddPlayer_ToMaleTournamentWithMalePlayer_ShouldSucceed`: Verifica que se pueda agregar un jugador masculino a un torneo masculino existente.
- `AddPlayer_ToMaleTournamentWithFemalePlayer_ShouldThrowArgumentException`: Verifica que se lance `ArgumentException` al intentar agregar una jugadora femenina a un torneo masculino.

_(Nota: Se podrían añadir pruebas análogas para torneos femeninos y el método `SetPlayers` siguiendo el mismo patrón)._

## 5. Validaciones Clave Verificadas

- **Consistencia de Género**: La principal validación es que el `PlayerType` de cada `Player` debe coincidir con el `TournamentType` del `Tournament`.
- **Tipo de Excepción**: En casos de inconsistencia de género, se espera que se lance una `ArgumentException`.
- **Mensajes de Excepción**: Se verifica que los mensajes de las excepciones sean informativos y reflejen la naturaleza del error (e.g., "Todos los jugadores deben ser del tipo Male.").
- **Parámetro de la Excepción**: Para `ArgumentException`, se verifica (cuando aplica, como en `AddPlayer`) que el `ParamName` sea el correcto (e.g., "player" o "players").

## 6. Consideraciones Adicionales

### 6.1. Aislamiento

Estas pruebas son puramente unitarias. No interactúan con bases de datos, servicios externos, ni la capa API. Esto asegura que los tests sean rápidos, fiables y se centren exclusivamente en la lógica de la entidad `Tournament`.

### 6.2. Datos de Prueba

Los datos de prueba (jugadores) se crean directamente en el código de los tests utilizando métodos helper (`CreateMalePlayer`, `CreateFemalePlayer`). Esto mantiene los tests auto-contenidos y fáciles de entender, sin dependencias de archivos externos para esta capa de pruebas.

## 7. Conclusión

Las pruebas unitarias implementadas para la validación de género en torneos aseguran que esta regla de negocio crítica funcione según lo esperado. Al probar directamente la entidad de dominio, se establece una base sólida para la corrección del sistema. Estos tests contribuyen a la calidad general del software, facilitan la refactorización segura y sirven como documentación viva del comportamiento esperado de la entidad `Tournament` respecto a esta validación.
