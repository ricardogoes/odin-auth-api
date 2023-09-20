using Odin.Auth.Domain.Exceptions;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Odin.Auth.Application.Helpers
{
    public class SortHelper
    {
        public static IEnumerable<T> ApplySort<T>(IEnumerable<T> data, string orderByQueryString) where T : class
        {
            if (!data.Any())
                return data;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    throw new BadRequestException("Invalid sort param. This field does not exist on resource entity.");

                var sortingOrder = param.EndsWith(" desc") ? "desc" : "asc";

                orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return data.AsQueryable().OrderBy(orderQuery);
        }
    }
}
