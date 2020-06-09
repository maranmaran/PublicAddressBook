using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Common.Extensions
{
    public static class CommonExtensions
    {
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

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

        public static IQueryable<TEntity> SortThenBy<TEntity, TKey>(
            this IQueryable<TEntity> collection,
            IEnumerable<Expression<Func<TEntity, TKey>>> selectors,
            string direction)
            where TEntity : class
        {
            if (direction == "asc")
            {
                var orderedCollection = collection.OrderBy(selectors.First());

                foreach (var selector in selectors.Skip(1))
                {
                    orderedCollection.ThenBy(selector);
                }

                return orderedCollection;
            }

            if (direction == "desc")
            {
                var orderedCollection = collection.OrderByDescending(selectors.First());

                foreach (var selector in selectors.Skip(1))
                {
                    orderedCollection.ThenByDescending(selector);
                }

                return orderedCollection;
            }

            return collection;
        }

        public static async Task<byte[]> ToByteArray(this Stream stream, CancellationToken cancellationToken = default)
        {
            await using var memStream = new MemoryStream();
            await stream.CopyToAsync(memStream, cancellationToken);

            return memStream.ToArray();
        }

    }
}