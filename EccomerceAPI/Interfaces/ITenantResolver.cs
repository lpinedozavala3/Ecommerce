namespace EccomerceAPI.Interfaces
{
    public interface ITenantResolver
    {
        Task<(Guid tiendaId, Guid emisorId)> ResolveAsync(HttpContext http);
    }
}
