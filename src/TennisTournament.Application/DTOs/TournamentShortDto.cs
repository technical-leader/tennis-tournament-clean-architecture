using System;
using System.Collections.Generic;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.DTOs
{
  /// <summary>
  /// DTO resumido para el listado general de torneos (sin partidos).
  /// </summary>
  public class TournamentShortDto
  {
    /// <summary>
    /// Identificador único del torneo.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tipo de torneo (Masculino o Femenino).
    /// </summary>
    public TournamentType Type { get; set; }

    /// <summary>
    /// Representación textual del tipo de torneo para mostrar.
    /// </summary>
    public string TypeDisplay => Type.ToString();

    /// <summary>
    /// Lista de identificadores de jugadores participantes en el torneo.
    /// </summary>
    public List<Guid> PlayerIds { get; set; } = new List<Guid>();

    /// <summary>
    /// Lista de nombres de jugadores participantes en el torneo para mostrar.
    /// </summary>
    public List<string> PlayerNames { get; set; } = new List<string>();

    /// <summary>
    /// Fecha de inicio del torneo.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Fecha de finalización del torneo.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Estado actual del torneo.
    /// </summary>
    public TournamentStatus Status { get; set; }

    /// <summary>
    /// Representación textual del estado actual del torneo para mostrar.
    /// </summary>
    public string StatusDisplay => Status.ToString();
  }
}
