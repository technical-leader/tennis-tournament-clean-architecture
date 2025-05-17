using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Converters;

public class JsonPlayerDtoConverter : JsonConverter<PlayerDto>
{
  public override PlayerDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    using (var document = JsonDocument.ParseValue(ref reader))
    {
      var root = document.RootElement;

      if (!root.TryGetProperty("name", out var nameProperty) || string.IsNullOrWhiteSpace(nameProperty.GetString()))
      {
        throw new JsonException("La propiedad 'name' es obligatoria y no puede estar vacía.");
      }

      if (root.TryGetProperty("playerType", out var playerTypeProperty))
      {
        var playerType = playerTypeProperty.GetString();
        if (playerType == null)
        {
          throw new JsonException("El tipo de jugador no se pudo determinar.");
        }

        switch (playerType.ToLowerInvariant())
        {
          case "male":
            return JsonSerializer.Deserialize<MalePlayerDto>(root.GetRawText(), options) ?? throw new JsonException("No se pudo deserializar el jugador masculino.");
          case "female":
            return JsonSerializer.Deserialize<FemalePlayerDto>(root.GetRawText(), options) ?? throw new JsonException("No se pudo deserializar la jugadora femenina.");
          default:
            throw new JsonException($"Tipo de jugador desconocido: {playerType}");
        }
      }

      throw new JsonException("No se pudo determinar el tipo de jugador.");
    }
  }

  public override void Write(Utf8JsonWriter writer, PlayerDto value, JsonSerializerOptions options)
  {
    JsonSerializer.Serialize(writer, value, value.GetType(), options);
  }
}
