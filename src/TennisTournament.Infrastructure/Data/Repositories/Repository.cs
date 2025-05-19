using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Implementación base genérica para repositorios.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TournamentDbContext _dbContext;

        /// <summary>
        /// Constructor con inyección del contexto de base de datos.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos.</param>
        protected Repository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        /// <returns>Lista de entidades.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Agrega una nueva entidad.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        /// <returns>La entidad agregada.</returns>
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        /// <returns>La entidad actualizada.</returns>
        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}