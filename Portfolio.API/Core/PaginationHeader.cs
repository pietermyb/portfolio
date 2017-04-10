namespace Portfolio.API.Core
{
    /// <summary>
    /// Adds a pagination header
    /// </summary>
    public class PaginationHeader
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>
        /// The items per page.
        /// </value>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        /// <value>
        /// The total items.
        /// </value>
        public int TotalItems { get; set; }
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        public int TotalPages { get; set; }

        /// <summary>
        /// PaginationHeader
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalItems"></param>
        /// <param name="totalPages"></param>
        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
    }
}
