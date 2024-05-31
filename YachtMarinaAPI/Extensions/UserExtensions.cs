using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> Search(this IQueryable<User> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query;
            }

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Username.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
