using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Queries
{
    public static partial class CustomerExtensions
    {
        #region Generated Extensions
        public static Repo.Data.Entities.Customer? GetByKey(this IQueryable<Repo.Data.Entities.Customer> queryable, long id)
        {
            if (queryable is null)
                throw new ArgumentNullException(nameof(queryable));

            if (queryable is DbSet<Repo.Data.Entities.Customer> dbSet)
                return dbSet.Find(id);

            return queryable.FirstOrDefault(q => q.Id == id);
        }

        public static ValueTask<Repo.Data.Entities.Customer?> GetByKeyAsync(this IQueryable<Repo.Data.Entities.Customer> queryable, long id)
        {
            if (queryable is null)
                throw new ArgumentNullException(nameof(queryable));

            if (queryable is DbSet<Repo.Data.Entities.Customer> dbSet)
                return dbSet.FindAsync(id);

            var task = queryable.FirstOrDefaultAsync(q => q.Id == id);
            return new ValueTask<Repo.Data.Entities.Customer?>(task);
        }

        #endregion

        public static async ValueTask<string> CheckExistingsForNew(this IQueryable<Repo.Data.Entities.Customer> queryable, Repo.Domain.Models.CustomerCreateModel custInfo)
        {
            if (queryable is null) throw new ArgumentNullException(nameof(queryable));
            if (custInfo is null) throw new ArgumentNullException(nameof(custInfo));

            if (queryable is DbSet<Repo.Data.Entities.Customer> dbSet)
            {
                var cQuery = dbSet.AsNoTracking().AsQueryable();

                if (await cQuery.AnyAsync(x => string.Equals(x.Nik, custInfo.Nik))) return $"NIK \"{custInfo.Nik}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Hp, custInfo.Hp))) return $"Phone number \"{custInfo.Hp}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Email.ToUpper(), custInfo.Email.ToUpper()))) return $"Email address \"{custInfo.Email.ToUpper()}\" is already used.";
            }
            else
            {
                var cQuery = queryable.AsNoTracking().AsQueryable();

                if (await cQuery.AnyAsync(x => string.Equals(x.Nik, custInfo.Nik))) return $"NIK \"{custInfo.Nik}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Hp, custInfo.Hp))) return $"Phone number \"{custInfo.Hp}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Email.ToUpper(), custInfo.Email.ToUpper()))) return $"Email address \"{custInfo.Email.ToUpper()}\" is already used.";
            }

            return "";
        }

        public static async ValueTask<string> CheckExistingsForUpdate(this IQueryable<Repo.Data.Entities.Customer> queryable, Repo.Domain.Models.CustomerUpdateModel custInfo)
        {
            if (queryable is null) throw new ArgumentNullException(nameof(queryable));
            if (custInfo is null) throw new ArgumentNullException(nameof(custInfo));

            if (queryable is DbSet<Repo.Data.Entities.Customer> dbSet)
            {
                var cQuery = dbSet.AsNoTracking().Where(x => !x.Id.Equals(custInfo.Id));

                if (await cQuery.AnyAsync(x => string.Equals(x.Nik, custInfo.Nik))) return $"NIK \"{custInfo.Nik}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Hp, custInfo.Hp))) return $"Phone number \"{custInfo.Hp}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Email.ToUpper(), custInfo.Email.ToUpper()))) return $"Email address \"{custInfo.Email.ToUpper()}\" is already used.";
            }
            else
            {
                var cQuery = queryable.AsNoTracking().Where(x => !x.Id.Equals(custInfo.Id));

                if (await cQuery.AnyAsync(x => string.Equals(x.Nik, custInfo.Nik))) return $"NIK \"{custInfo.Nik}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Hp, custInfo.Hp))) return $"Phone number \"{custInfo.Hp}\" is already used.";
                if (await cQuery.AnyAsync(x => string.Equals(x.Email.ToUpper(), custInfo.Email.ToUpper()))) return $"Email address \"{custInfo.Email.ToUpper()}\" is already used.";
            }

            return "";
        }
    }
}