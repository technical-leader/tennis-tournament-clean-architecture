using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TennisTournament.Domain.Interfaces
{
    /// <summary>
    /// Interfaz genérica para repositorios.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        /// <returns>Lista de entidades.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Agrega una nueva entidad.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        /// <returns>La entidad agregada.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        /// <returns>La entidad actualizada.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        Task DeleteAsync(T entity);
    }
}