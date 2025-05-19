namespace TennisTournament.Domain.Enums
{
    /// <summary>
    /// Enumeración que representa el estado de un torneo.
    /// </summary>
    public enum TournamentStatus
    {
        /// <summary>
        /// Torneo programado pero aún no iniciado.
        /// </summary>
        Scheduled = 0,

        /// <summary>
        /// Torneo en progreso.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Torneo completado.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Torneo cancelado.
        /// </summary>
        Cancelled = 3
    }
}