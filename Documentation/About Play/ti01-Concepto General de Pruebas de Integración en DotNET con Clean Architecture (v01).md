## Índice

- [Concepto General de Pruebas de Integración en .NET 9 con Clean Architecture](#concepto-general-de-pruebas-de-integración-en-net-9-con-clean-architecture)
- [Enfoque en `Data`: Inicialización y Gestión de Datos en Pruebas](#enfoque-en-data-inicialización-y-gestión-de-datos-en-pruebas)
  - Estrategia para Inicialización de Datos en Pruebas

---

## **Concepto General de Pruebas de Integración en .NET 9 con Clean Architecture**

**Las pruebas de integración verifican cómo interactúan diferentes componentes de la aplicación, asegurando que los módulos trabajen correctamente en conjunto.**  
En el contexto de `.NET 9` con **Arquitectura Limpia (Clean Architecture)**, las pruebas de integración validan la comunicación entre **capa de aplicación**, **infraestructura** y **base de datos**, garantizando que la solución opere sin errores cuando los componentes interactúan.

🔹 **Estructura del directorio `TennisTournament.Tests.Integration`**  
Cada subdirectorio cumple una función específica en la validación de la integridad del sistema:

- `bin`: Archivos de compilación del proyecto de pruebas.
- `Config`: Configuración de las pruebas (puede incluir variables de entorno y configuración de test runners).
- `Controllers`: Pruebas sobre los controladores de la API (validando endpoints y respuestas HTTP).
- `Data`: Configuración de datos iniciales para pruebas.
- `Features`: Pruebas sobre características específicas del sistema (casos de uso y lógica de negocio).
- `Framework`: Utilidades para el framework de pruebas (puede contener helpers o configuraciones comunes).
- `Logs`: Registro de pruebas ejecutadas (para análisis de errores y comportamiento).

---

## **Enfoque en `Data`: Inicialización y Gestión de Datos en Pruebas**

**El directorio `Data` en las pruebas de integración está enfocado en la configuración y carga de datos iniciales para validar escenarios de prueba.**

- **Permite poblar la base de datos con información controlada.**
- **Garantiza que las pruebas siempre inicien con datos previsibles.**

🔹 **Archivos dentro de `Data`:**

- **`SeedData.cs`**:
  - Contiene datos predefinidos para alimentar la base de datos antes de ejecutar pruebas.
  - Permite la creación de jugadores, torneos y matches de prueba.
- **`SeedDataExtensions.cs`**:
  - Proporciona métodos de extensión para facilitar la inserción de datos en el contexto de Entity Framework.
  - Se usa en la configuración del `DbContext` de pruebas para asegurar que la base de datos tenga los datos necesarios.

---

### ** Estrategia para Inicialización de Datos en Pruebas**

**Antes de ejecutar las pruebas, se requiere un ambiente de datos estable.**  
🔹 **Pasos clave en la inicialización de datos:**

1. **Limpiar la base de datos de pruebas** (si existen datos previos).
2. **Cargar datos iniciales desde `SeedData.cs`**.
3. **Extender `DbContext` con `SeedDataExtensions.cs`** para insertar registros iniciales.
4. **Ejecutar pruebas sobre la base de datos preconfigurada.**

**Esto permite poblar la base de datos antes de ejecutar pruebas, garantizando datos de prueba consistentes.**

---

**Resumen de la estrategia de pruebas de integración en `TennisTournament.Tests.Integration`**

✔ **`Data` se encarga de la inicialización y gestión de datos para pruebas.**  
✔ **`SeedData.cs` define los registros preconfigurados.**  
✔ **`SeedDataExtensions.cs` facilita la inserción de datos en `DbContext`.**  
✔ **Se ejecutan pruebas en un ambiente con datos controlados, asegurando estabilidad en los casos de uso.**
