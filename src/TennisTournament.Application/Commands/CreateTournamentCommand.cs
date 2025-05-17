using System;
using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Commands
{
    /// <summary>
    /// Comando para crear un nuevo torneo.
    /// </summary>
    public class CreateTournamentCommand : IRequest<TournamentDto>
    {
        /// <summary>
        /// Tipo de torneo (Masculino o Femenino).
        /// </summary>
        public TournamentType Type { get; set; }

        /// <summary>
        /// Lista de identificadores de jugadores participantes en el torneo.
        /// </summary>
        public List<Guid> PlayerIds { get; set; }

        /// <summary>
        /// Fecha de inicio del torneo.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public CreateTournamentCommand()
        {
            PlayerIds = new List<Guid>();
            StartDate = DateTime.Now;
        }
    }
}