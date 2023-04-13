using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ByuEgyptSite
{
    /// <summary>
    /// Receives a list of objects and creates a new paginated list of the objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {
        // The page index
        public int PageIndex { get; set; }

        // Total number of pages
        public int TotalPages { get; set; }

        // Constructor
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        // Booleans to determine previous and next pages
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Creates and returns the paginated list of objects
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PaginatedList<T> Create(List<T> source,
            int pageIndex, int pageSize)
        {
            // count the objects
            var count = source.Count;

            // skip and take the specified amount of pages
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            // Return the paginated list
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
