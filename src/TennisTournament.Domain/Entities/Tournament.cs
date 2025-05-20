using System;
using System.Collections.Generic;
using System.Linq;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Entities
{
    /// <summary>
    /// Clase que representa un torneo de tenis.
    /// </summary>
    public class Tournament
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
        /// Lista interna de jugadores participantes en el torneo.
        /// </summary>
        private List<Player> _players = new List<Player>();

        /// <summary>
        /// Lista de jugadores participantes en el torneo (solo lectura).
        /// </summary>
        public IReadOnlyCollection<Player> Players => _players.AsReadOnly();

        /// <summary>
        /// Lista interna de partidos del torneo.
        /// </summary>
        private List<Match> _matches = new List<Match>();

        /// <summary>
        /// Lista de partidos del torneo (solo lectura).
        /// </summary>
        public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();

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
        /// Constructor por defecto.
        /// </summary>
        public Tournament()
        {
            Id = Guid.NewGuid();
            StartDate = DateTime.UtcNow;
            Status = TournamentStatus.Scheduled;
        }

        /// <summary>
        /// Constructor con parámetros.
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <param name="players">Lista de jugadores.</param>
        public Tournament(TournamentType type, List<Player> players)
        {
            Id = Guid.NewGuid();
            Type = type;
            ValidatePlayers(players, type);
            _players = players ?? throw new ArgumentNullException(nameof(players));
            StartDate = DateTime.UtcNow;
            Status = TournamentStatus.Scheduled;
        }

        /// <summary>
        /// Añade un jugador al torneo.
        /// </summary>
        /// <param name="player">Jugador a añadir.</param>
        /// <exception cref="InvalidOperationException">Si el torneo ya ha comenzado.</exception>
        /// <exception cref="ArgumentException">Si el jugador no es del tipo correcto.</exception>
        /// <exception cref="ArgumentNullException">Si el jugador es nulo.</exception>
        public void AddPlayer(Player player)
        {
            if (Status != TournamentStatus.Scheduled)
                throw new InvalidOperationException("No se pueden añadir jugadores a un torneo que ya ha comenzado.");

            if (player == null)
                throw new ArgumentNullException(nameof(player));

            if (player.PlayerType != (PlayerType)Type)
                throw new ArgumentException($"El jugador debe ser del tipo {Type}.", nameof(player));

            _players.Add(player);
        }

        /// <summary>
        /// Establece la lista de jugadores del torneo.
        /// </summary>
        /// <param name="players">Lista de jugadores.</param>
        /// <exception cref="InvalidOperationException">Si el torneo ya ha comenzado, el número de jugadores no es potencia de 2.</exception>
        /// <exception cref="ArgumentException">Si los jugadores no son del tipo correcto.</exception>
        /// <exception cref="ArgumentNullException">Si la lista de jugadores es nula.</exception>
        public void SetPlayers(IEnumerable<Player> players)
        {
            if (Status != TournamentStatus.Scheduled)
                throw new InvalidOperationException("No se pueden modificar los jugadores de un torneo que ya ha comenzado.");

            if (players == null)
                throw new ArgumentNullException(nameof(players));

            var playersList = players.ToList();

            // Verificar que el número de jugadores sea potencia de 2
            if (!IsPowerOfTwo(playersList.Count))
                throw new InvalidOperationException("El número de jugadores debe ser una potencia de 2.");

            // Verificar que todos los jugadores sean del tipo correcto según el torneo
            if (playersList.Any(p => p.PlayerType != (PlayerType)Type))
                throw new ArgumentException($"Todos los jugadores deben ser del tipo {Type}.", nameof(players));

            _players = playersList;
        }

        /// <summary>
        /// Añade un partido al torneo.
        /// </summary>
        /// <param name="match">Partido a añadir.</param>
        /// <exception cref="InvalidOperationException">Si el torneo ha finalizado o el partido no pertenece a este torneo.</exception>
        /// <exception cref="ArgumentNullException">Si el partido es nulo.</exception>
        public void AddMatch(Match match)
        {
            // if (Status == TournamentStatus.Completed || Status == TournamentStatus.Cancelled)
            //     throw new InvalidOperationException("No se pueden añadir partidos a un torneo finalizado o cancelado.");

            if (Status == TournamentStatus.Cancelled)
                throw new InvalidOperationException("No se pueden añadir partidos a un torneo cancelado.");

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (match.TournamentId != Id)
                throw new InvalidOperationException("El partido debe pertenecer a este torneo.");

            _matches.Add(match);
        }

        /// <summary>
        /// Valida que la lista de jugadores sea válida para un torneo.
        /// </summary>
        /// <param name="players">Lista de jugadores a validar.</param>
        /// <param name="type">Tipo de torneo.</param>
        private void ValidatePlayers(List<Player> players, TournamentType type)
        {
            if (players == null || !players.Any())
                throw new ArgumentException("La lista de jugadores no puede estar vacía.", nameof(players));

            // Verificar que el número de jugadores sea potencia de 2
            int playerCount = players.Count;
            if ((playerCount & (playerCount - 1)) != 0)
                throw new ArgumentException("El número de jugadores debe ser una potencia de 2.", nameof(players));

            // Verificar que los jugadores sean del tipo correcto según el torneo
            bool allPlayersValid = type == TournamentType.Male
                ? players.TrueForAll(p => p.PlayerType == PlayerType.Male)
                : players.TrueForAll(p => p.PlayerType == PlayerType.Female);

            if (!allPlayersValid)
                throw new ArgumentException($"Todos los jugadores deben ser del tipo {type}.", nameof(players));
        }

        /// <summary>
        /// Genera los emparejamientos iniciales para la primera ronda del torneo.
        /// </summary>
        /// <returns>Lista de partidos de la primera ronda.</returns>
        /// <exception cref="InvalidOperationException">Si el torneo ya ha comenzado o finalizado.</exception>
        public List<Match> GenerateInitialMatches()
        {
            if (Status != TournamentStatus.Scheduled)
                throw new InvalidOperationException("No se pueden generar emparejamientos para un torneo que ya ha comenzado o finalizado.");

            // Mezclar aleatoriamente los jugadores para los emparejamientos iniciales
            var shuffledPlayers = _players.OrderBy(p => Guid.NewGuid()).ToList();
            var firstRoundMatches = new List<Match>();

            for (int i = 0; i < shuffledPlayers.Count; i += 2)
            {
                var match = new Match
                {
                    Player1 = shuffledPlayers[i],
                    Player2 = shuffledPlayers[i + 1],
                    Player1Id = shuffledPlayers[i].Id,
                    Player2Id = shuffledPlayers[i + 1].Id,
                    Round = 1,
                    TournamentId = Id,
                    Tournament = this,
                    MatchDate = DateTime.UtcNow
                };
                firstRoundMatches.Add(match);
            }

            _matches.AddRange(firstRoundMatches);
            Status = TournamentStatus.InProgress;
            return firstRoundMatches;
        }

        /// <summary>
        /// Genera los emparejamientos para la siguiente ronda del torneo.
        /// </summary>
        /// <param name="currentRoundWinners">Lista de ganadores de la ronda actual.</param>
        /// <param name="nextRound">Número de la siguiente ronda.</param>
        /// <returns>Lista de partidos de la siguiente ronda.</returns>
        /// <exception cref="InvalidOperationException">Si el torneo no está en progreso.</exception>
        /// <exception cref="ArgumentNullException">Si la lista de ganadores es nula.</exception>
        public List<Match> GenerateNextRoundMatches(List<Player> currentRoundWinners, int nextRound)
        {
            if (Status != TournamentStatus.InProgress)
                throw new InvalidOperationException("El torneo no está en progreso.");

            if (currentRoundWinners == null)
                throw new ArgumentNullException(nameof(currentRoundWinners));

            if (currentRoundWinners.Count < 2)
            {
                // Si solo queda un ganador, el torneo ha terminado
                Status = TournamentStatus.Completed;
                EndDate = DateTime.UtcNow;
                return new List<Match>();
            }

            var nextRoundMatches = new List<Match>();
            for (int i = 0; i < currentRoundWinners.Count; i += 2)
            {
                var match = new Match
                {
                    Player1 = currentRoundWinners[i],
                    Player2 = currentRoundWinners[i + 1],
                    Player1Id = currentRoundWinners[i].Id,
                    Player2Id = currentRoundWinners[i + 1].Id,
                    Round = nextRound,
                    TournamentId = Id,
                    Tournament = this,
                    MatchDate = DateTime.UtcNow
                };
                nextRoundMatches.Add(match);
            }

            _matches.AddRange(nextRoundMatches);
            return nextRoundMatches;
        }

        /// <summary>
        /// Verifica si un número es potencia de 2.
        /// </summary>
        /// <param name="x">Número a verificar.</param>
        /// <returns>True si es potencia de 2, false en caso contrario.</returns>
        private bool IsPowerOfTwo(int x)
        {
            return x > 0 && (x & (x - 1)) == 0;
        }
    }
}