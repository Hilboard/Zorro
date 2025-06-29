using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials;

public static class MapToDTOQuery
{
    public static (QueryContext, object?) MapToDTO<TDTOable, TDTO>(
        this (QueryContext context, TDTOable entity) input,
        object? argsObject = null
    )
        where TDTOable : class, IDataTransferObject<TDTO>
    {
        return (input.context, input.entity.MapToDTO(argsObject));
    }
}