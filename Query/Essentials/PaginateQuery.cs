namespace Zorro.Query.Essentials;

public static class PaginateQuery
{
    public static ArgHttpQueryContext<Pagination<TEntity>> Paginate<TEntity>(
        this ArgHttpQueryContext<IEnumerable<TEntity>> context,
        int startIndex,
        int pageSize,
        bool reverse = false
    )
    {
        var items = Paginate(context.arg, startIndex, pageSize, out int endIndex, out int totalCount, reverse);
        bool lastPage = endIndex == totalCount - 1;
        Pagination<TEntity> paginationObj = new Pagination<TEntity>(items, totalCount, endIndex, lastPage);

        context.TryLogElapsedTime(nameof(PaginateQuery));

        return context.PassArg(paginationObj);
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

    public class Pagination<TItem>
    {
        public IEnumerable<TItem> items { get; set; }
        public int totalCount { get; set; }
        public int endIndex { get; set; }
        public bool lastPage { get; set; }

        public Pagination(IEnumerable<TItem> items, int totalCount, int endIndex, bool lastPage)
        {
            this.items = items;
            this.totalCount = totalCount;
            this.endIndex = endIndex;
            this.lastPage = lastPage;
        }
    }
}