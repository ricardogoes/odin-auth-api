using Odin.Auth.Domain.Models;

namespace Odin.Auth.Api.Models
{
    public class PaginatedApiResponse<TItemData> where TItemData : class
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        public IEnumerable<TItemData> Items { get; set; } = Enumerable.Empty<TItemData>();

        public PaginatedApiResponse()
        { }

        public PaginatedApiResponse(PaginatedListOutput<TItemData> data)
        {
            PageNumber = data.PageNumber;
            PageSize = data.PageSize;
            TotalPages = data.TotalPages;
            TotalRecords = data.TotalItems;
            Items = data.Items;
        }
    }
}
