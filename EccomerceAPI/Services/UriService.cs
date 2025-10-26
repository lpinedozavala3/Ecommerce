using Database.Filters;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace EccomerceAPI.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            Uri endpointUri = new Uri(string.Concat(_baseUri, route));
            string modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }

        public Uri GetPageUriEmisor(Guid emisorId, PaginationFilter filter, string route)
        {
            Uri endpointUri = new Uri(string.Concat(_baseUri, route));
            string modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "emisorId", emisorId.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
