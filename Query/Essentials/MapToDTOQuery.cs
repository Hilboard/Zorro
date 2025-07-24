using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials;

public static class MapToDTOQuery
{
    public static ArgQueryContext<TDTO> MapToDTO<TEntity, TDTO>(
        this ArgQueryContext<TEntity> context
    )
        where TEntity : class, IDTO<TDTO>
    {
        return context.PassArg(context.arg.MapToDTO(context));
    }
}