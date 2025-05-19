using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de partidos.
    /// </summary>
    public class MatchRepository : IMatchRepository
    {
        private readonly TournamentDbContext _dbContext;

        /// <summary>
        /// Constructor con inyección del contexto de base de datos.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos.</param>
        public MatchRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Obtiene todos los partidos.
        /// </summary>
        /// <returns>Lista de partidos.</returns>
        public async Task<IEnumerable<Match>> GetAllAsync()
        {
            return await _dbContext.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Include(m => m.Tournament)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un partido por su identificador.
        /// </summary>
        /// <param name="id">Identificador del partido.</param>
        /// <returns>Partido encontrado o null si no existe.</returns>
        public async Task<Match?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Obtiene los partidos de un torneo específico.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <returns>Lista de partidos del torneo.</returns>
        public async Task<IEnumerable<Match>> GetByTournamentIdAsync(Guid tournamentId)
        {
            return await _dbContext.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Where(m => m.TournamentId == tournamentId)
                .OrderBy(m => m.Round)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los partidos de una ronda específica de un torneo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <param name="round">Número de ronda.</param>
        /// <returns>Lista de partidos de la ronda especificada.</returns>
        public async Task<IEnumerable<Match>> GetByTournamentAndRoundAsync(Guid tournamentId, int round)
        {
            return await _dbContext.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Where(m => m.TournamentId == tournamentId && m.Round == round)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los partidos en los que ha participado un jugador específico.
        /// </summary>
        /// <param name="playerId">Identificador del jugador.</param>
        /// <returns>Lista de partidos en los que ha participado el jugador.</returns>
        public async Task<IEnumerable<Match>> GetByPlayerIdAsync(Guid playerId)
        {
            return await _dbContext.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Include(m => m.Tournament)
                .Where(m => m.Player1Id == playerId || m.Player2Id == playerId)
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }

        /// <summary>
        /// Añade un nuevo partido.
        /// </summary>
        /// <param name="match">Partido a añadir.</param>
        /// <returns>Partido añadido.</returns>
        public async Task<Match> AddAsync(Match match)
        {
            await _dbContext.Matches.AddAsync(match);
            await _dbContext.SaveChangesAsync();
            return match;
        }

        /// <summary>
        /// Añade múltiples partidos.
        /// </summary>
        /// <param name="matches">Lista de partidos a añadir.</param>
        /// <returns>Lista de partidos añadidos.</returns>
        public async Task<IEnumerable<Match>> AddRangeAsync(IEnumerable<Match> matches)
        {
            await _dbContext.Matches.AddRangeAsync(matches);
            await _dbContext.SaveChangesAsync();
            return matches;
        }

        /// <summary>
        /// Actualiza un partido existente.
        /// </summary>
        /// <param name="match">Partido con los datos actualizados.</param>
        /// <returns>Partido actualizado.</returns>
        public async Task<Match> UpdateAsync(Match match)
        {
            _dbContext.Entry(match).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return match;
        }

        /// <summary>
        /// Elimina un partido por su identificador.
        /// </summary>
        /// <param name="id">Identificador del partido a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var match = await _dbContext.Matches.FindAsync(id);
            if (match == null)
                return false;

            _dbContext.Matches.Remove(match);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}