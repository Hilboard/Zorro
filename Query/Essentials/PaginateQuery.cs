namespace Zorro.Query.Essentials;

public static class PaginateQuery
{
    public static ArgQueryContext<object> Paginate<TEntity>(
        this ArgQueryContext<IEnumerable<TEntity>> context,
        int startIndex,
        int pageSize,
        bool reverse = false
    )
    {
        var items = Paginate(context.arg, startIndex, pageSize, out int endIndex, out int totalCount, reverse);
        return context.PassArg(WrapResult());

        object WrapResult()
        {
            return new
            {
                items,
                totalCount,
                endIndex,
                lastPage = endIndex == totalCount - 1,
            };
        }
    }

    private static IEnumerable<TEntity> Paginate<TEntity>(
        IEnumerable<TEntity> items,
        int startIndex,
        int pageSize,
        out int endIndex,
        out int totalCount,
        bool reverse = false
    )
    {
        items = items.Where(entity => entity != null).Select(dto => dto!);

        totalCount = items.Count();

        endIndex = startIndex + pageSize - 1;
        if (endIndex < 0)
            endIndex = 0;
        else if (endIndex > totalCount - 1)
            endIndex = totalCount - 1;

        if (totalCount == 0)
        {
            return Array.Empty<TEntity>();
        }

        if (reverse)
            items = items.Reverse();

        items = items.Skip(startIndex).Take(pageSize);
        return items;
    }
}