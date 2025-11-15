using System;

namespace Database.DTOs
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri? NextPage { get; set; }
        public Uri? PreviousPage { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, string? message = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = message;
            Errors = null;
        }


    }
}
