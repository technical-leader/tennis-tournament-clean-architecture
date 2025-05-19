## Técnicas Avanzadas para Transformación de DTOs: Uso de Mappers en .NET 9

Para optimizar el uso de **DTOs (Data Transfer Objects)** en una API moderna en el sistema, se pueden implementar **mappers dinámicos y validaciones personalizadas** para mejorar la seguridad, eficiencia y flexibilidad en la conversión de datos.

---

## Índice

- [Técnicas Avanzadas para Transformación de DTOs: Uso de Mappers en .NET 9](#técnicas-avanzadas-para-transformación-de-dtos-uso-de-mappers-en-net-9)
- [Índice](#índice)
  - [1. Mappers Dinámicos: Flexibilidad en la Conversión de DTOs](#1-mappers-dinámicos-flexibilidad-en-la-conversión-de-dtos)
  - [2. Uso de AutoMapper para Transformación Avanzada](#2-uso-de-automapper-para-transformación-avanzada)
  - [3. Mapeo Condicional y Personalizado](#3-mapeo-condicional-y-personalizado)
  - [4. Validaciones Personalizadas en DTOs](#4-validaciones-personalizadas-en-dtos)
  - [5. Ejemplo Completo: Integración de Mappers y Validaciones](#5-ejemplo-completo-integración-de-mappers-y-validaciones)
  - [6. Resumen](#6-resumen)

---

### 1. Mappers Dinámicos: Flexibilidad en la Conversión de DTOs

**¿Por qué usar mappers dinámicos en el sistema?**

- Permiten **convertir entidades a DTOs sin duplicar código** en el sistema.
- Adaptan la transformación **según la necesidad del cliente** en el sistema.
- **Evitan escribir código manual repetitivo** para cada conversión en el sistema.

**Ejemplo con conversión dinámica en el sistema:**

```csharp
public static T MapToDto<T>(object source) where T : class, new()
{
    var json = JsonSerializer.Serialize(source);
    return JsonSerializer.Deserialize<T>(json)!;
}
```

**Este método en el sistema convierte cualquier entidad en un DTO sin definir manualmente la transformación.**

---

### 2. Uso de AutoMapper para Transformación Avanzada

**AutoMapper permite transformar entidades y DTOs de manera eficiente en el sistema.**
**Ejemplo de configuración en `.NET 9` en el sistema:**

```csharp
public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioDto>();
    }
}
```

**Esto en el sistema habilita la conversión automática entre `Usuario` y `UsuarioDto`.**

**Ejemplo de uso en un servicio en el sistema:**

```csharp
public class UsuarioService
{
    private readonly IMapper _mapper;

    public UsuarioService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UsuarioDto ObtenerUsuarioDto(Usuario usuario)
    {
        return _mapper.Map<UsuarioDto>(usuario);
    }
}
```

**Esto en el sistema elimina código manual y permite una conversión robusta y mantenible.**

---

### 3. Mapeo Condicional y Personalizado

**AutoMapper también permite personalizar la transformación en el sistema** en función de condiciones dinámicas.

**Ejemplo: Excluir `Password` en el mapeo en el sistema**

```csharp
CreateMap<Usuario, UsuarioDto>()
    .ForMember(dest => dest.Password, opt => opt.Ignore());
```

**Esto en el sistema asegura que `Password` nunca se exponga en el DTO.**

**Ejemplo: Transformar datos en el mapeo en el sistema**

```csharp
CreateMap<Usuario, UsuarioDto>()
    .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre.ToUpper()));
```

**Aquí, en el sistema se convierte `Nombre` a mayúsculas antes de enviarlo al DTO.**

---

### 4. Validaciones Personalizadas en DTOs

**En .NET 9, se pueden aplicar validaciones específicas en DTOs en el sistema.**

**Ejemplo con `DataAnnotations` en el sistema:**

```csharp
public class UsuarioDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }

    [Range(0, 100, ErrorMessage = "El nivel debe estar entre 0 y 100")]
    public int Nivel { get; set; }
}
```

**Estas validaciones en el sistema aseguran que los datos sean correctos antes de ser procesados.**

**Validaciones adicionales con `IValidatableObject` en el sistema:**

```csharp
public class UsuarioDto : IValidatableObject
{
    public string Nombre { get; set; }
    public string Correo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Correo.Contains("@"))
        {
            yield return new ValidationResult("El correo debe ser válido", new[] { nameof(Correo) });
        }
    }
}
```

**Esto en el sistema permite validaciones avanzadas con reglas dinámicas dentro del DTO.**

---

### 5. Ejemplo Completo: Integración de Mappers y Validaciones

**Cómo usar mappers y validaciones en una API en el sistema:**

```csharp
[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IMapper _mapper;

    public UsuarioController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult CrearUsuario([FromBody] UsuarioDto usuarioDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var usuario = _mapper.Map<Usuario>(usuarioDto);
        return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuario.Id }, _mapper.Map<UsuarioDto>(usuario));
    }

    [HttpGet("{id}")]
    public ActionResult<UsuarioDto> ObtenerUsuario(Guid id)
    {
        var usuario = new Usuario { Id = id, Nombre = "Test", FechaAlta = DateTime.UtcNow };
        return Ok(_mapper.Map<UsuarioDto>(usuario));
    }
}
```

**Este controlador en el sistema usa mapeo dinámico con `AutoMapper` y validaciones personalizadas.**

---

### 6. Resumen

- Los mappers dinámicos mejoran la flexibilidad en la conversión de DTOs en el sistema.
- AutoMapper permite transformar datos sin esfuerzo en el sistema.
- El mapeo condicional excluye datos sensibles y personaliza respuestas en el sistema.
- Las validaciones aseguran que los DTOs solo contengan datos correctos en el sistema.
