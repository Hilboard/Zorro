using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials;

public static class MapToDTOQuery
{
    public static ArgQueryContext<TDTO> MapToDTO<TEntity, TDTO>(
        this ArgQueryContext<TEntity> context
    )
        where TEntity : class, IDTO<TDTO>
    {
        TDTO dto = context.arg.MapToDTO(context);
        context.TryLogElapsedTime(nameof(MapToDTOQuery));
        return context.PassArg(dto);
    }
}