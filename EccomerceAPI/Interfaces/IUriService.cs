using Database.Filters;

namespace EccomerceAPI.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
        public Uri GetPageUriEmisor(Guid emisorId, PaginationFilter filter, string route);
    }
}
