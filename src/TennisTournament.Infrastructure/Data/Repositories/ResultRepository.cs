using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Interfaces;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de resultados.
    /// </summary>
    public class ResultRepository : IResultRepository
    {
        private readonly TournamentDbContext _dbContext;

        /// <summary>
        /// Constructor con inyección del contexto de base de datos.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos.</param>
        public ResultRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Obtiene todos los resultados.
        /// </summary>
        /// <returns>Lista de resultados.</returns>
        public async Task<IEnumerable<Result>> GetAllAsync()
        {
            return await _dbContext.Results
                .Include(r => r.Tournament)
                .Include(r => r.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player2)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un resultado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del resultado.</param>
        /// <returns>Resultado encontrado o null si no existe.</returns>
        public async Task<Result?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Results
                .Include(r => r.Tournament)
                .Include(r => r.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player2)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Obtiene el resultado de un torneo específico.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <returns>Resultado del torneo o null si no existe.</returns>
        public async Task<Result?> GetByTournamentIdAsync(Guid tournamentId)
        {
            return await _dbContext.Results
                .Include(r => r.Tournament)
                .Include(r => r.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player2)
                .FirstOrDefaultAsync(r => r.TournamentId == tournamentId);
        }

        /// <summary>
        /// Obtiene resultados por tipo de torneo.
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <returns>Lista de resultados de torneos del tipo especificado.</returns>
        public async Task<IEnumerable<Result>> GetByTournamentTypeAsync(TournamentType type)
        {
            return await _dbContext.Results
                .Include(r => r.Tournament)
                .Include(r => r.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player2)
                .Where(r => r.Tournament.Type == type)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene resultados por rango de fechas de finalización.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        /// <returns>Lista de resultados dentro del rango de fechas.</returns>
        public async Task<IEnumerable<Result>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Results
                .Include(r => r.Tournament)
                .Include(r => r.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player2)
                .Where(r => r.Tournament.EndDate >= startDate && r.Tournament.EndDate <= endDate)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene resultados por ganador.
        /// </summary>
        /// <param name="winnerId">Identificador del jugador ganador.</param>
        /// <returns>Lista de resultados donde el jugador especificado es el ganador.</returns>
        public async Task<IEnumerable<Result>> GetByWinnerIdAsync(Guid winnerId)
        {
            return await _dbContext.Results
                .Include(r => r.Tournament)
                .Include(r => r.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Winner)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Player2)
                .Where(r => r.WinnerId == winnerId)
                .ToListAsync();
        }

        /// <summary>
        /// Añade un nuevo resultado.
        /// </summary>
        /// <param name="result">Resultado a añadir.</param>
        /// <returns>Resultado añadido.</returns>
        public async Task<Result> AddAsync(Result result)
        {
            await _dbContext.Results.AddAsync(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// Actualiza un resultado existente.
        /// </summary>
        /// <param name="result">Resultado con los datos actualizados.</param>
        /// <returns>Resultado actualizado.</returns>
        public async Task<Result> UpdateAsync(Result result)
        {
            _dbContext.Entry(result).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// Elimina un resultado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del resultado a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _dbContext.Results.FindAsync(id);
            if (result == null)
                return false;

            _dbContext.Results.Remove(result);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}