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
    /// Implementación del repositorio de jugadores.
    /// </summary>
    public class PlayerRepository : IPlayerRepository
    {
        private readonly TournamentDbContext _dbContext;

        /// <summary>
        /// Constructor con inyección del contexto de base de datos.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos.</param>
        public PlayerRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Obtiene todos los jugadores.
        /// </summary>
        /// <returns>Lista de jugadores.</returns>
        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _dbContext.Players.ToListAsync();
        }

        /// <summary>
        /// Obtiene un jugador por su identificador.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <returns>Jugador encontrado o null si no existe.</returns>
        public async Task<Player?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Players.FindAsync(id);
        }

        /// <summary>
        /// Añade un nuevo jugador.
        /// </summary>
        /// <param name="player">Jugador a añadir.</param>
        /// <returns>Jugador añadido.</returns>
        public async Task<Player> AddAsync(Player player)
        {
            await _dbContext.Players.AddAsync(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }

        /// <summary>
        /// Actualiza un jugador existente.
        /// </summary>
        /// <param name="player">Jugador con los datos actualizados.</param>
        /// <returns>Jugador actualizado.</returns>
        public async Task<Player> UpdateAsync(Player player)
        {
            _dbContext.Entry(player).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return player;
        }

        /// <summary>
        /// Elimina un jugador por su identificador.
        /// </summary>
        /// <param name="id">Identificador del jugador a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var player = await _dbContext.Players.FindAsync(id);
            if (player == null)
                return false;

            _dbContext.Players.Remove(player);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Obtiene todos los jugadores masculinos.
        /// </summary>
        /// <returns>Lista de jugadores masculinos.</returns>
        public async Task<IEnumerable<MalePlayer>> GetAllMalePlayersAsync()
        {
            return await _dbContext.MalePlayers.ToListAsync();
        }

        /// <summary>
        /// Obtiene todas las jugadoras femeninas.
        /// </summary>
        /// <returns>Lista de jugadoras femeninas.</returns>
        public async Task<IEnumerable<FemalePlayer>> GetAllFemalePlayersAsync()
        {
            return await _dbContext.FemalePlayers.ToListAsync();
        }

        /// <summary>
        /// Obtiene jugadores por nombre (búsqueda parcial).
        /// </summary>
        /// <param name="name">Nombre o parte del nombre a buscar.</param>
        /// <returns>Lista de jugadores que coinciden con la búsqueda.</returns>
        public async Task<IEnumerable<Player>> GetByNameAsync(string name)
        {
            return await _dbContext.Players
                .Where(p => p.Name.Contains(name))
                .ToListAsync();
        }
    }
}