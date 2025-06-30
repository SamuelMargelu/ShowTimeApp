using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;
using System.Linq.Expressions;

namespace ShowTime.Services.Implementations
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();

            if (!result.Any())
            {
                throw new InvalidOperationException("No Items Found");
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes)
        {
            var result = await _repository.GetAllIncludingAsync(includes);

            if (!result.Any())
            {
                throw new InvalidOperationException("No Items Found");
            }

            return result;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Item with ID {id} not found.");
            }

            return result;
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
