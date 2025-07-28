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
        context.TryLogElapsedTime(nameof(MapToDTOsQuery));
        return context.PassArg(DTOs);
    }

    public static ArgQueryContext<PaginateQuery.Pagination<TDTO>> MapToDTOs<TEntity, TDTO>(
        this ArgQueryContext<PaginateQuery.Pagination<TEntity>> context
    )
        where TEntity : class, IDTO<TDTO>
    {
        var items = context.arg.items.Select(e => e.MapToDTO(context));
        PaginateQuery.Pagination<TDTO> paginationObj = new PaginateQuery.Pagination<TDTO>(
            items,
            context.arg.totalCount,
            context.arg.endIndex,
            context.arg.lastPage
        );
        context.TryLogElapsedTime(nameof(MapToDTOsQuery));
        return context.PassArg(paginationObj);
    }
}