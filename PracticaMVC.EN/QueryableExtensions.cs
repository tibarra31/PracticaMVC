using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.EN
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            PropertyInfo property;
            Expression propertyAccess;
            if (sortExpression.Contains('.'))
            {
                // support to be sorted on child fields.
                string[] childProperties = sortExpression.Split('.');
                property = type.GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i].Replace(" DESC", ""));
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(T).GetProperty(sortExpression.Replace(" DESC", ""));
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            bool ascending = true;

            if (sortExpression.Contains("DESC"))
                ascending = false;

            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
                                                             ascending ? "OrderBy" : "OrderByDescending",
                                                             new[] { type, property.PropertyType }, source.Expression,
                                                             Expression.Quote(orderByExp));
            //return  source.OrderBy(x => orderByExp);
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
