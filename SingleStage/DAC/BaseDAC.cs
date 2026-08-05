using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    //              class ClassName<T>     : SomeInterface<T>   where T : constraint
    public abstract class BaseDAC<TEntity> : IDAC<TEntity>      where TEntity : class

    {
        protected readonly SingleStageMvvmContext _context;

        protected BaseDAC(SingleStageMvvmContext context)
        {
            _context = context;
        }

        // notes:
        // Set<TEntity>() means: "Give me the DbSet for whatever entity type I provide."

        // read all
        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return _context.Set<TEntity>().ToListAsync();
        }

        // read one
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        // add
        public virtual async Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
        }

        // update
        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        // delete
        public virtual async Task DeleteAsync(int id)
        {
            TEntity? entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
