using System;
using System.Collections.Generic;
using System.Linq;

namespace TennisTournament.Domain.Entities
{
    /// <summary>
    /// Clase que representa el resultado de un torneo.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Identificador único del resultado.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador del torneo al que pertenece este resultado.
        /// </summary>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Identificador del jugador ganador del torneo.
        /// </summary>
        public Guid WinnerId { get; set; }

        /// <summary>
        /// Fecha de finalización del torneo.
        /// </summary>
        public DateTime CompletionDate { get; set; }

        /// <summary>
        /// Torneo al que pertenece este resultado.
        /// </summary>
        public required Tournament Tournament { get; set; }

        /// <summary>
        /// Jugador ganador del torneo.
        /// </summary>
        public required Player Winner { get; set; }

        /// <summary>
        /// Lista interna de partidos jugados en el torneo.
        /// </summary>
        private List<Match> _matches = new List<Match>();

        /// <summary>
        /// Lista de partidos jugados en el torneo (solo lectura).
        /// </summary>
        public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();

        /// <summary>
        /// Indica si el resultado está finalizado.
        /// </summary>
        public bool IsFinalized { get; private set; }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Result()
        {
            Id = Guid.NewGuid();
            CompletionDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor con parámetros.
        /// </summary>
        /// <param name="tournament">Torneo al que pertenece este resultado.</param>
        /// <param name="winner">Jugador ganador del torneo.</param>
        /// <param name="matches">Lista de partidos jugados en el torneo.</param>
        /// <exception cref="ArgumentNullException">Si el torneo, el ganador o la lista de partidos es nula.</exception>
        /// <exception cref="ArgumentException">Si el ganador no es uno de los jugadores del torneo.</exception>
        public Result(Tournament tournament, Player winner, List<Match> matches)
        {
            Id = Guid.NewGuid();
            Tournament = tournament ?? throw new ArgumentNullException(nameof(tournament));
            TournamentId = tournament.Id;
            
            // Validar que el ganador sea uno de los jugadores del torneo
            if (!tournament.Players.Contains(winner))
                throw new ArgumentException("El ganador debe ser uno de los jugadores del torneo.", nameof(winner));
            
            Winner = winner ?? throw new ArgumentNullException(nameof(winner));
            WinnerId = winner.Id;
            
            if (matches != null)
            {
                _matches = matches;
            }
            
            CompletionDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Añade un partido al historial de partidos.
        /// </summary>
        /// <param name="match">Partido a añadir.</param>
        /// <exception cref="InvalidOperationException">Si el resultado ya está finalizado o el partido no pertenece al mismo torneo.</exception>
        /// <exception cref="ArgumentNullException">Si el partido es nulo.</exception>
        public void AddMatch(Match match)
        {
            if (IsFinalized)
                throw new InvalidOperationException("No se pueden añadir partidos a un resultado finalizado.");
                
            if (match == null)
                throw new ArgumentNullException(nameof(match));
                
            if (match.TournamentId != TournamentId)
                throw new InvalidOperationException("El partido debe pertenecer al mismo torneo que el resultado.");
                
            _matches.Add(match);
        }

        /// <summary>
        /// Establece la lista de partidos del resultado.
        /// </summary>
        /// <param name="matches">Lista de partidos.</param>
        /// <exception cref="InvalidOperationException">Si el resultado ya está finalizado o algún partido no pertenece al mismo torneo.</exception>
        /// <exception cref="ArgumentNullException">Si la lista de partidos es nula.</exception>
        public void SetMatches(IEnumerable<Match> matches)
        {
            if (IsFinalized)
                throw new InvalidOperationException("No se pueden modificar los partidos de un resultado finalizado.");
                
            if (matches == null)
                throw new ArgumentNullException(nameof(matches));
                
            var matchesList = matches.ToList();
            
            // Validar que todos los partidos pertenezcan al mismo torneo
            if (matchesList.Any(m => m.TournamentId != TournamentId))
                throw new InvalidOperationException("Todos los partidos deben pertenecer al mismo torneo que el resultado.");
                
            _matches = matchesList;
        }

        /// <summary>
        /// Marca el resultado como finalizado, impidiendo futuras modificaciones.
        /// </summary>
        public void MarkAsFinalized()
        {
            IsFinalized = true;
        }
    }
}