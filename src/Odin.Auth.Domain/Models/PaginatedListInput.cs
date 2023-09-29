namespace Odin.Auth.Domain.Models
{ 
    public abstract class PaginatedListInput
    {

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public string? Sort { get; private set; }

        protected PaginatedListInput()
        {
            PageNumber = 1;
            PageSize = 10;
            Sort = "lastUpdatedAt desc";
        }

        protected PaginatedListInput(int pageNumber, int pageSize, string? sort)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
            Sort = sort ?? "lastUpdatedAt desc";
        }
    }
}
