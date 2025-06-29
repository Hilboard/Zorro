using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials;

public static class MapToDTOsQuery
{
    public static (QueryContext, IEnumerable<TDTO?>) MapToDTOs<TDTOable, TDTO>(
        this (QueryContext context, IEnumerable<TDTOable> entity) input,
        Func<TDTOable, object>? argsObjectBuilder = null
    )
        where TDTOable : class, IDataTransferObject<TDTO>
    {
        var DTOs = input.entity.Select(e => e.MapToDTO(argsObjectBuilder!(e)));
        return (input.context, DTOs);
    }

    public static (QueryContext, IEnumerable<TDTO?>) MapToDTOs<TDTOable, TDTO>(
        this (QueryContext context, IEnumerable<TDTOable> entity) input
    )
        where TDTOable : class, IDataTransferObject<TDTO>
    {
        var DTOs = input.entity.Select(e => e.MapToDTO());
        return (input.context, DTOs);
    }
}