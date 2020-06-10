using System;
using System.Linq;
using System.Linq.Expressions;

namespace Backend.Common.Extensions
{
    public static class CommonExtensions
    {

        /// <summary>
        /// Sorts prop by given direction
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="collection"></param>
        /// <param name="selector"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Sort<TEntity, TKey>(
            this IQueryable<TEntity> collection,
            Expression<Func<TEntity, TKey>> selector,
            string direction)
            where TEntity : class
        {
            if (direction == "asc")
            {
                return collection.OrderBy(selector);
            }

            if (direction == "desc")
            {
                return collection.OrderByDescending(selector);
            }

            return collection;
        }

    }
}