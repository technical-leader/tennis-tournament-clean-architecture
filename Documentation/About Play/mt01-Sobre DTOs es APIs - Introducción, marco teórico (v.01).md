## Sobre DTOs en APIs: _introducción, marco teórico_

Los **DTOs (Data Transfer Objects)** son fundamentales para estructurar APIs seguras y mantenibles en **.NET 9**. **Separan la representación de datos interna de la lógica del servicio** en el sistema, evitando exposiciones innecesarias de la base de datos.

---

## Índice

- [Sobre DTOs en APIs: _introducción, marco teórico_](#sobre-dtos-en-apis-introducción-marco-teórico)
- [Índice](#índice)
  - [1. ¿Qué es un DTO y por qué usarlo?](#1-qué-es-un-dto-y-por-qué-usarlo)
  - [2. Ventajas de los DTOs en una API](#2-ventajas-de-los-dtos-en-una-api)
  - [3. Ejemplo optimizado de entidad vs. DTO en .NET 9](#3-ejemplo-optimizado-de-entidad-vs-dto-en-net-9)
  - [4. Cómo convertir entre entidad y DTO con un mapper](#4-cómo-convertir-entre-entidad-y-dto-con-un-mapper)
  - [5. Buenas prácticas en el uso de DTOs](#5-buenas-prácticas-en-el-uso-de-dtos)
  - [6. Resumen](#6-resumen)

---

### 1. ¿Qué es un DTO y por qué usarlo?

Un **DTO (Data Transfer Object)** es un objeto diseñado exclusivamente para **transferir datos entre el cliente y servidor** en una aplicación **.NET 9**.
No contiene lógica de negocio, solo almacena datos para la comunicación entre capas en el sistema.

**Razones para usar DTOs en el sistema:**

- Separar la estructura de respuesta de la entidad interna.
- No exponer directamente la base de datos.
- Controlar qué datos se envían y reciben.
- Facilitar cambios en el modelo sin afectar la API pública.

Ejemplo práctico: Si una entidad `Usuario` en el sistema contiene datos sensibles como contraseñas, **un DTO evita exponer información crítica**.

---

### 2. Ventajas de los DTOs en una API

**Comparación entre usar entidades directamente vs. usar DTOs en el sistema:**

| Sin DTO (Exposición de Entidad)       | Con DTO (Protección de Datos)         |
| ------------------------------------- | ------------------------------------- |
| Expone `Password` y `FechaAlta`.      | Solo muestra `Nombre` y `FechaAlta`.  |
| Ligado a la estructura interna de BD. | Independiente de cambios internos.    |
| Si el modelo cambia, la API se rompe. | Adaptable sin afectar la API pública. |

---

### 3. Ejemplo optimizado de entidad vs. DTO en .NET 9

Supongamos que en el sistema se tiene la entidad `Usuario`:

```csharp
public class Usuario
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Password { get; set; }
    public DateTime FechaAlta { get; set; }
}
```

**Problema:** La entidad contiene datos sensibles (`Password`) que no deben exponerse en la API del sistema.

**Solución: Usar un DTO para controlar la información en el sistema**

```csharp
public class UsuarioDto
{
    public string Nombre { get; set; }
    public DateTime FechaAlta { get; set; }
}
```

**Ventajas:**

- Solo envía datos necesarios (`Nombre`, `FechaAlta`).
- Oculta la `Password`, evitando vulnerabilidades.
- Separación entre datos internos y la respuesta pública.

---

### 4. Cómo convertir entre entidad y DTO con un mapper

Usar un **mapper** facilita la conversión entre entidades y DTOs en el sistema. Esto se puede realizar manualmente o con herramientas como **AutoMapper**.

**Ejemplo con conversión manual:**

```csharp
public static UsuarioDto ToDto(Usuario usuario)
{
    return new UsuarioDto
    {
        Nombre = usuario.Nombre,
        FechaAlta = usuario.FechaAlta
    };
}
```

**Ejemplo con AutoMapper (automatización):**

```csharp
public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioDto>();
    }
}
```

**Usar `AutoMapper` simplifica el código y mejora la mantenibilidad en el sistema.**

---

### 5. Buenas prácticas en el uso de DTOs

- Se crea un DTO separado para cada request y response importante en el sistema.
- Se evita usar entidades directamente en el controlador en el sistema.
- Se utiliza un mapper (como `AutoMapper`) para convertir entre entidad y DTO en el sistema.
- Los DTOs se mantienen libres de lógica de negocio en el sistema.

**Ejemplo en un controlador del sistema sin usar la entidad directamente:**

```csharp
[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
    {
        _usuarioService = usuarioService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public ActionResult<UsuarioDto> GetUsuario(Guid id)
    {
        var usuario = _usuarioService.FindById(id);
        if (usuario == null) return NotFound();

        return Ok(_mapper.Map<UsuarioDto>(usuario));
    }
}
```

**Esto mantiene la API del sistema limpia y segura sin exponer datos innecesarios.**

---

### 6. Resumen

- Los DTOs protegen la API del sistema y evitan exponer entidades internas.
- Separan la lógica interna del modelo público en el sistema.
- Facilitan cambios sin afectar a los clientes externos del sistema.
- Usar `AutoMapper` mejora la conversión entre entidad y DTO en el sistema.
