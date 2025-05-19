## Impacto en la Performance al Usar DTOs y AutoMapper

El uso de **DTOs y AutoMapper** en APIs de .NET mejora la organización y seguridad en el sistema, pero también tiene un impacto en la **performance** que se debe considerar.

---

## Índice

- [Impacto en la Performance al Usar DTOs y AutoMapper](#impacto-en-la-performance-al-usar-dtos-y-automapper)
- [Índice](#índice)
  - [1. Impacto en la performance de usar DTOs](#1-impacto-en-la-performance-de-usar-dtos)
  - [2. Costo de conversión entre entidades y DTOs](#2-costo-de-conversión-entre-entidades-y-dtos)
  - [3. Performance de AutoMapper vs. Mapeo manual](#3-performance-de-automapper-vs-mapeo-manual)
  - [4. Optimización del uso de DTOs y AutoMapper](#4-optimización-del-uso-de-dtos-y-automapper)
  - [5. Estrategias avanzadas para mejorar la eficiencia](#5-estrategias-avanzadas-para-mejorar-la-eficiencia)
  - [6. Resumen](#6-resumen)

---

### 1. Impacto en la performance de usar DTOs

**Ventajas:**

- Evitan la exposición directa de entidades, mejorando la seguridad en el sistema.
- Reducción de datos transmitidos, mejorando la eficiencia en la red.
- Flexibilidad para ajustar respuestas sin modificar la base de datos en el sistema.

**Costo en rendimiento:**

- Mayor uso de memoria, ya que los DTOs agregan capas adicionales de objetos en el sistema.
- Transformaciones adicionales en el pipeline de la API, impactando el procesamiento en el sistema.

**Ejemplo: Serialización de Entidad vs. DTO en el sistema**

```csharp
// Serialización de una entidad sin DTO (más directa)
return Ok(usuario);

// Serialización con DTO (requiere conversión previa)
return Ok(_mapper.Map<UsuarioDto>(usuario));
```

El uso de DTOs introduce una transformación adicional antes de enviar la respuesta en el sistema.

---

### 2. Costo de conversión entre entidades y DTOs

La conversión de una entidad a DTO consume tiempo y memoria en el sistema, especialmente si hay objetos anidados o grandes colecciones.

**Ejemplo de conversión manual en el sistema:**

```csharp
public UsuarioDto ConvertirAUsuarioDto(Usuario usuario)
{
    return new UsuarioDto
    {
        Nombre = usuario.Nombre,
        FechaAlta = usuario.FechaAlta
    };
}
```

**Ejemplo con AutoMapper en el sistema:**

```csharp
public UsuarioDto ConvertirConAutoMapper(Usuario usuario)
{
    return _mapper.Map<UsuarioDto>(usuario);
}
```

AutoMapper puede optimizar la conversión en el sistema, pero cada llamada tiene un costo adicional.

---

### 3. Performance de AutoMapper vs. Mapeo manual

**Comparación en velocidad en el sistema:**

| Método           | Performance                                                    |
| ---------------- | -------------------------------------------------------------- |
| **Mapeo manual** | Más rápido, menos consumo de memoria.                          |
| **AutoMapper**   | Más conveniente, pero usa reflexión interna y consume más CPU. |

**Ejemplo de diferencia en performance con grandes colecciones en el sistema:**

```csharp
// Mapeo manual
var usuariosDto = usuarios.Select(u => new UsuarioDto { Nombre = u.Nombre, FechaAlta = u.FechaAlta }).ToList();

// AutoMapper
var usuariosDto = _mapper.Map<List<UsuarioDto>>(usuarios);
```

En grandes volúmenes de datos, el mapeo manual suele ser más eficiente en el sistema.

---

### 4. Optimización del uso de DTOs y AutoMapper

**Mejorar performance con AutoMapper en el sistema:**

- Usar `Projection()` en consultas LINQ para evitar cargar objetos completos antes de mapear.
- Configurar `AutoMapper` con `PreserveReferences()` para evitar ciclos de referencias.
- Evitar múltiples instancias de `_mapper`, reutilizando la misma configuración global en el sistema.

**Ejemplo de `Projection()` para mejorar eficiencia en el sistema:**

```csharp
var usuariosDto = _context.Usuarios
    .ProjectTo<UsuarioDto>(_mapper.ConfigurationProvider)
    .ToList();
```

Esto permite convertir directamente desde la base de datos sin crear objetos adicionales en el sistema.

---

### 5. Estrategias avanzadas para mejorar la eficiencia

- Usar DTOs solo cuando sea necesario en el sistema. Si una entidad no necesita protección, se puede enviar directamente.
- Evitar `AutoMapper` en colecciones grandes en el sistema. Para conjuntos grandes de datos, se recomienda usar mapeo manual.
- Reducir serialización innecesaria en el sistema. Usar JSON compacto y evitar enviar datos no usados.

---

### 6. Resumen

- Los DTOs mejoran la seguridad y claridad en el sistema, pero tienen un costo de memoria y procesamiento.
- AutoMapper es conveniente, pero consume más CPU que el mapeo manual en grandes volúmenes de datos en el sistema.
- Proyectar DTOs directamente desde la base de datos mejora la eficiencia (`ProjectTo<>`) en el sistema.
- Evitar el uso de DTOs innecesarios y optimizar la serialización reduce el impacto en la performance del sistema.
