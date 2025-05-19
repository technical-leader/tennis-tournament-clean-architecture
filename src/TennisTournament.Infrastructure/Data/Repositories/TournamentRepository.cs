using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de torneos.
    /// </summary>
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentDbContext _dbContext;

        /// <summary>
        /// Constructor con inyección del contexto de base de datos.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos.</param>
        public TournamentRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Obtiene todos los torneos.
        /// </summary>
        /// <returns>Lista de torneos.</returns>
        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await _dbContext.Tournaments
                .Include(t => t.Players)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un torneo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del torneo.</param>
        /// <returns>Torneo encontrado o null si no existe.</returns>
        public async Task<Tournament?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tournaments
                .Include(t => t.Players)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Player2)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Winner)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Obtiene torneos por tipo (masculino o femenino).
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <returns>Lista de torneos del tipo especificado.</returns>
        public async Task<IEnumerable<Tournament>> GetByTypeAsync(TournamentType type)
        {
            return await _dbContext.Tournaments
                .Include(t => t.Players)
                .Where(t => t.Type == type)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene torneos por rango de fechas.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        /// <returns>Lista de torneos dentro del rango de fechas.</returns>
        public async Task<IEnumerable<Tournament>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Tournaments
                .Include(t => t.Players)
                .Where(t => t.StartDate >= startDate && t.StartDate <= endDate)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene torneos por estado.
        /// </summary>
        /// <param name="status">Estado del torneo.</param>
        /// <returns>Lista de torneos con el estado especificado.</returns>
        public async Task<IEnumerable<Tournament>> GetByStatusAsync(TournamentStatus status)
        {
            return await _dbContext.Tournaments
                .Include(t => t.Players)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        /// <summary>
        /// Añade un nuevo torneo.
        /// </summary>
        /// <param name="tournament">Torneo a añadir.</param>
        /// <returns>Torneo añadido.</returns>
        public async Task<Tournament> AddAsync(Tournament tournament)
        {
            await _dbContext.Tournaments.AddAsync(tournament);
            await _dbContext.SaveChangesAsync();
            return tournament;
        }

        /// <summary>
        /// Actualiza un torneo existente.
        /// </summary>
        /// <param name="tournament">Torneo con los datos actualizados.</param>
        /// <returns>Torneo actualizado.</returns>
        public async Task<Tournament> UpdateAsync(Tournament tournament)
        {
            // 1. Cargar el torneo original con sus partidos y jugadores
            var existingTournament = await _dbContext.Tournaments
                .Include(t => t.Matches)
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == tournament.Id);

            if (existingTournament == null)
                throw new InvalidOperationException("Torneo no encontrado.");

            // 2. Eliminar todos los partidos existentes (si hay)
            var matchesToRemove = existingTournament.Matches.ToList();
            foreach (var match in matchesToRemove)
            {
                _dbContext.Matches.Remove(match);
            }

            // 3. Agregar los nuevos partidos
            foreach (var match in tournament.Matches)
            {
                // Importante: Evitar referencia cíclica, pero asignar el torneo correcto
                match.Tournament = existingTournament;
                _dbContext.Matches.Add(match);
            }

            // 4. Actualizar propiedades del torneo (excepto Matches y Players)
            _dbContext.Entry(existingTournament).CurrentValues.SetValues(tournament);

            await _dbContext.SaveChangesAsync();
            return existingTournament;
        }

        /// <summary>
        /// Elimina un torneo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del torneo a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var tournament = await _dbContext.Tournaments.FindAsync(id);
            if (tournament == null)
                return false;

            _dbContext.Tournaments.Remove(tournament);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}