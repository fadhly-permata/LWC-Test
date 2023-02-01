using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Queries
{
    public static partial class RentExtensions
    {
        #region Generated Extensions
        public static Repo.Data.Entities.Rent? GetByKey(this IQueryable<Repo.Data.Entities.Rent> queryable, long id)
        {
            if (queryable is null)
                throw new ArgumentNullException(nameof(queryable));

            if (queryable is DbSet<Repo.Data.Entities.Rent> dbSet)
                return dbSet.Find(id);

            return queryable.FirstOrDefault(q => q.Id == id);
        }

        public static ValueTask<Repo.Data.Entities.Rent?> GetByKeyAsync(this IQueryable<Repo.Data.Entities.Rent> queryable, long id)
        {
            if (queryable is null)
                throw new ArgumentNullException(nameof(queryable));

            if (queryable is DbSet<Repo.Data.Entities.Rent> dbSet)
                return dbSet.FindAsync(id);

            var task = queryable.FirstOrDefaultAsync(q => q.Id == id);
            return new ValueTask<Repo.Data.Entities.Rent?>(task);
        }

        #endregion
    }
}
