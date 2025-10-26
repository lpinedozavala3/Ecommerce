using Database.DTOs;
using Database.Filters;
using EccomerceAPI.Interfaces;

namespace EccomerceAPI.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            PagedResponse<List<T>> respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            double totalPages = totalRecords / (double)validFilter.PageSize;
            int roundedTotalPages = (int)Math.Ceiling(totalPages);
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                    : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                    : null;
            respose.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }

        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, Guid emisorId, string route)
        {
            PagedResponse<List<T>> respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            double totalPages = totalRecords / (double)validFilter.PageSize;
            int roundedTotalPages = (int)Math.Ceiling(totalPages);

            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUriEmisor(emisorId,
                        new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                    : null;

            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUriEmisor(emisorId,
                        new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                    : null;

            respose.FirstPage = uriService.GetPageUriEmisor(emisorId, new PaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUriEmisor(emisorId, new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
