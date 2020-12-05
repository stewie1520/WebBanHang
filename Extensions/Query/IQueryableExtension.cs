using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Extensions.Query
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, Expression<Func<T, object>> keySelector, bool ascending)
        {
            var selectorBody = keySelector.Body;

            if (selectorBody.NodeType == ExpressionType.Convert)
                selectorBody = ((UnaryExpression)selectorBody).Operand;

            var selector = Expression.Lambda(selectorBody, keySelector.Parameters);

            var queryBody = Expression.Call(typeof(Queryable),
                ascending ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), selectorBody.Type },
                source.Expression, Expression.Quote(selector));

            return source.Provider.CreateQuery<T>(queryBody);
        }
    }
}
