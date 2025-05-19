# Estrategia de Pruebas Unitarias

## Índice

- [Estrategia de Pruebas Unitarias](#estrategia-de-pruebas-unitarias)
  - [Índice](#índice)
  - [Introducción](#introducción)
  - [Categorías de Pruebas Unitarias](#categorías-de-pruebas-unitarias)
    - [1. Pruebas de Controladores API](#1-pruebas-de-controladores-api)
    - [2. Pruebas de Handlers (CQRS)](#2-pruebas-de-handlers-cqrs)
    - [3. Pruebas de Validadores](#3-pruebas-de-validadores)
    - [4. Pruebas de Servicios de Dominio](#4-pruebas-de-servicios-de-dominio)
    - [5. Pruebas de Repositorios](#5-pruebas-de-repositorios)
  - [Mejores Prácticas](#mejores-prácticas)
    - [Estructura de Pruebas](#estructura-de-pruebas)
    - [Nomenclatura](#nomenclatura)
    - [Organización del Código](#organización-del-código)
    - [Herramientas Utilizadas](#herramientas-utilizadas)
  - [Implementación](#implementación)
    - [Ejemplo Conceptual de Prueba de Controlador](#ejemplo-conceptual-de-prueba-de-controlador)
    - [Ejemplo Conceptual de Prueba de Handler](#ejemplo-conceptual-de-prueba-de-handler)
  - [Cobertura de Pruebas](#cobertura-de-pruebas)
  - [Conclusión](#conclusión)

---

## Introducción

Este documento describe la estrategia de pruebas unitarias implementada en el sistema TennisTournament, enfocándose en las mejores prácticas para implementar pruebas efectivas según su propósito.

## Categorías de Pruebas Unitarias

Las pruebas unitarias se organizan en las siguientes categorías según su propósito:

### 1. Pruebas de Controladores API

**Propósito**: Verificar que los controladores API manejen correctamente las solicitudes HTTP, apliquen validaciones adecuadas y devuelvan los códigos de estado HTTP apropiados.

**Enfoque**:

- Aislar el controlador mediante el uso de mocks para sus dependencias (principalmente `IMediator`).
- Verificar que los comandos y consultas se construyan correctamente en el sistema.
- Comprobar que los resultados HTTP sean los esperados (200 OK, 201 Created, 400 BadRequest, 404 NotFound, etc.) en el sistema.
- Validar que las respuestas contengan los datos esperados en el sistema.

**Referencia Conceptual**: Pruebas para `PlayersController`.

### 2. Pruebas de Handlers (CQRS)

**Propósito**: Verificar que los handlers de comandos y consultas procesen correctamente las solicitudes y produzcan los resultados esperados.

**Enfoque**:

- Aislar los handlers mediante mocks para repositorios y servicios en el sistema.
- Verificar la lógica de negocio implementada en los handlers.
- Comprobar el mapeo correcto entre entidades y DTOs en el sistema.
- Validar el manejo de casos excepcionales en el sistema.

### 3. Pruebas de Validadores

**Propósito**: Verificar que los validadores de FluentValidation apliquen correctamente las reglas de negocio.

**Enfoque**:

- Probar cada regla de validación individualmente en el sistema.
- Verificar casos válidos e inválidos.
- Comprobar mensajes de error personalizados.

### 4. Pruebas de Servicios de Dominio

**Propósito**: Verificar que los servicios de dominio implementen correctamente la lógica de negocio central.

**Enfoque**:

- Probar la lógica de negocio aislada de la infraestructura en el sistema.
- Verificar cálculos, algoritmos y reglas de negocio.
- Comprobar el manejo de casos límite y excepcionales.

### 5. Pruebas de Repositorios

**Propósito**: Verificar que los repositorios interactúen correctamente con la capa de persistencia.

**Enfoque**:

- Utilizar bases de datos en memoria para pruebas aisladas en el sistema.
- Verificar operaciones CRUD básicas.
- Comprobar consultas personalizadas.
- Validar el manejo de transacciones y concurrencia.

## Mejores Prácticas

### Estructura de Pruebas

Cada prueba unitaria debe seguir el patrón AAA (Arrange-Act-Assert):

1. **Arrange**: Configurar los objetos y establecer las condiciones necesarias para la prueba en el sistema.
2. **Act**: Realizar la acción que se está probando en el sistema.
3. **Assert**: Verificar que los resultados son los esperados en el sistema.

### Nomenclatura

Las pruebas deben seguir una convención de nomenclatura clara:
Se utiliza un formato que describa el método bajo prueba, el escenario específico y el resultado esperado. Por ejemplo, un nombre de prueba podría ser `GetById_ConIdExistente_DebeRetornarOkConJugador` o `ActualizarJugadorMasculino_ConTipoDeJugadorInvalido_DebeRetornarBadRequest`.

### Organización del Código

- Agrupar pruebas relacionadas usando regiones o clases anidadas en el sistema.
- Utilizar métodos de ayuda para configuraciones comunes.
- Implementar fixtures de prueba para configuraciones compartidas.

### Herramientas Utilizadas

- **xUnit**: Framework de pruebas
- **Moq**: Biblioteca para crear mocks
- **FluentAssertions**: Biblioteca para aserciones más legibles y expresivas

## Implementación

### Ejemplo Conceptual de Prueba de Controlador

Un ejemplo de prueba para un controlador podría verificar que, al solicitar un jugador por un ID existente, el controlador devuelva un resultado HTTP `OK` (200) junto con los datos del jugador.
En la fase **Arrange**, se prepararía un ID de jugador y un DTO de jugador simulado. Se configuraría el mock del mediador para que, al recibir una consulta con ese ID, devuelva el DTO simulado.
En la fase **Act**, se invocaría el método del controlador correspondiente (por ejemplo, `GetById`) con el ID preparado.
En la fase **Assert**, se verificaría que la respuesta sea un `OkObjectResult` y que el DTO contenido en la respuesta coincida con el DTO simulado.

### Ejemplo Conceptual de Prueba de Handler

Para un handler de CQRS, una prueba podría verificar que, al procesar una consulta válida para obtener un jugador, el handler devuelva el DTO del jugador correspondiente.
En la fase **Arrange**, se prepararía un ID de jugador y una entidad de jugador simulada. Se configurarían los mocks del repositorio (para devolver la entidad simulada al ser consultado por el ID) y del mapeador (para convertir la entidad simulada al DTO esperado). Se crearía la consulta con el ID y se instanciaría el handler con los mocks.
En la fase **Act**, se invocaría el método `Handle` del handler con la consulta.
En la fase **Assert**, se verificaría que el resultado no sea nulo y que el ID del DTO devuelto coincida con el ID original.

## Cobertura de Pruebas

Se recomienda mantener una cobertura de pruebas unitarias de al menos:

- 90% para la capa de Aplicación
- 80% para la capa de Dominio
- 70% para la capa de API
- 60% para la capa de Infraestructura

## Conclusión

La implementación de pruebas unitarias siguiendo esta estrategia garantizará la calidad del código en el sistema, facilitará la detección temprana de errores y proporcionará una documentación viva del comportamiento esperado del sistema.

Las pruebas unitarias son la base de la pirámide de pruebas y deben complementarse con pruebas de integración y pruebas de extremo a extremo para garantizar la calidad completa del sistema.
