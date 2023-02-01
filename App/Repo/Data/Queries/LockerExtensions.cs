using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Queries
{
    public static partial class LockerExtensions
    {
        #region Generated Extensions
        public static Repo.Data.Entities.Locker? GetByKey(this IQueryable<Repo.Data.Entities.Locker> queryable, long id)
        {
            if (queryable is null)
                throw new ArgumentNullException(nameof(queryable));

            if (queryable is DbSet<Repo.Data.Entities.Locker> dbSet)
                return dbSet.Find(id);

            return queryable.FirstOrDefault(q => q.Id == id);
        }

        public static ValueTask<Repo.Data.Entities.Locker?> GetByKeyAsync(this IQueryable<Repo.Data.Entities.Locker> queryable, long id)
        {
            if (queryable is null)
                throw new ArgumentNullException(nameof(queryable));

            if (queryable is DbSet<Repo.Data.Entities.Locker> dbSet)
                return dbSet.FindAsync(id);

            var task = queryable.FirstOrDefaultAsync(q => q.Id == id);
            return new ValueTask<Repo.Data.Entities.Locker?>(task);
        }

        #endregion

        public static async ValueTask<string> CheckExistingForNew(this IQueryable<Repo.Data.Entities.Locker> queryable, Repo.Domain.Models.LockerCreateModel lockInfo)
        {
            if (queryable is null) throw new ArgumentNullException(nameof(queryable));
            if (lockInfo is null) throw new ArgumentNullException(nameof(lockInfo));

            if(queryable is DbSet<Repo.Data.Entities.Locker> dbSet) {
                if (await dbSet.AsNoTracking().AnyAsync(x => string.Equals(x.Number, lockInfo.Number))) return $"Locker number \"{lockInfo.Number}\" is already used.";
            } else {
                if (await queryable.AsNoTracking().AnyAsync(x => string.Equals(x.Number, lockInfo.Number))) return $"Locker number \"{lockInfo.Number}\" is already used.";
            }

            return "";
        }

        public static async ValueTask<string> CheckExistingForUpdate(this IQueryable<Repo.Data.Entities.Locker> queryable, Repo.Domain.Models.LockerUpdateModel lockInfo)
        {
            if (queryable is null) throw new ArgumentNullException(nameof(queryable));
            if (lockInfo is null) throw new ArgumentNullException(nameof(lockInfo));

            if(queryable is DbSet<Repo.Data.Entities.Locker> dbSet) {
                if (await dbSet.AsNoTracking().Where(x => !x.Id.Equals(lockInfo.Id)).AnyAsync(x => string.Equals(x.Number, lockInfo.Number))) return $"Locker number \"{lockInfo.Number}\" is already used.";
            } else {
                if (await queryable.AsNoTracking().Where(x => !x.Id.Equals(lockInfo.Id)).AnyAsync(x => string.Equals(x.Number, lockInfo.Number))) return $"Locker number \"{lockInfo.Number}\" is already used.";
            }

            return "";
        }
    }
}
