using Microsoft.EntityFrameworkCore;

namespace PersonalBlogApp.Helpers
{
    public static class OrderUtils
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(x => EF.Property<object>(x, propertyName));
        }

        public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(x => EF.Property<object>(x, propertyName));
        }
    }
}
