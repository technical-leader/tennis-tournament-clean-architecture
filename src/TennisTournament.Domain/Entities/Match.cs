using System;

namespace TennisTournament.Domain.Entities
{
    /// <summary>
    /// Clase que representa un partido de tenis entre dos jugadores.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Identificador único del partido.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador del primer jugador.
        /// </summary>
        public Guid Player1Id { get; set; }

        /// <summary>
        /// Identificador del segundo jugador.
        /// </summary>
        public Guid Player2Id { get; set; }

        /// <summary>
        /// Identificador del jugador ganador (puede ser nulo si el partido no se ha jugado).
        /// </summary>
        public Guid? WinnerId { get; set; }

        /// <summary>
        /// Primer jugador del partido.
        /// </summary>
        public required Player Player1 { get; set; }

        /// <summary>
        /// Segundo jugador del partido.
        /// </summary>
        public required Player Player2 { get; set; }

        /// <summary>
        /// Jugador ganador del partido.
        /// </summary>
        public Player? Winner { get; set; }

        /// <summary>
        /// Ronda del torneo en la que se juega este partido.
        /// </summary>
        public int Round { get; set; }

        /// <summary>
        /// Fecha y hora en la que se juega el partido.
        /// </summary>
        public DateTime MatchDate { get; set; }

        /// <summary>
        /// Identificador del torneo al que pertenece este partido.
        /// </summary>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Torneo al que pertenece este partido.
        /// </summary>
        public required Tournament Tournament { get; set; }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Match()
        {
            Id = Guid.NewGuid();
            MatchDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor con parámetros.
        /// </summary>
        /// <param name="player1">Primer jugador.</param>
        /// <param name="player2">Segundo jugador.</param>
        /// <param name="round">Ronda del torneo.</param>
        /// <param name="tournamentId">Identificador del torneo.</param>
        public Match(Player player1, Player player2, int round, Guid tournamentId)
        {
            Id = Guid.NewGuid();
            Player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
            Player2 = player2 ?? throw new ArgumentNullException(nameof(player2));
            Player1Id = player1.Id;
            Player2Id = player2.Id;
            Round = round;
            TournamentId = tournamentId;
            MatchDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Determina el ganador del partido basado en las puntuaciones calculadas para cada jugador.
        /// </summary>
        /// <param name="luckFactor1">Factor de suerte para el primer jugador.</param>
        /// <param name="luckFactor2">Factor de suerte para el segundo jugador.</param>
        /// <returns>El jugador ganador.</returns>
        public Player DetermineWinner(double luckFactor1, double luckFactor2)
        {
            double score1 = Player1.CalculateTotalScore(luckFactor1);
            double score2 = Player2.CalculateTotalScore(luckFactor2);

            Winner = score1 > score2 ? Player1 : Player2;
            WinnerId = Winner.Id;
            return Winner;
        }
    }
}