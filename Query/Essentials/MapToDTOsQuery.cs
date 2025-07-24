using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials;

public static class MapToDTOsQuery
{
    public static ArgQueryContext<IEnumerable<TDTO>> MapToDTOs<TEntity, TDTO>(
        this ArgQueryContext<IEnumerable<TEntity>> context
    )
        where TEntity : class, IDTO<TDTO>
    {
        var DTOs = context.arg.Select(e => e.MapToDTO(context));
        return context.PassArg(DTOs);
    }
}