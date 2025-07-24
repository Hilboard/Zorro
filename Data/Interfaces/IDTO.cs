using Zorro.Query;

namespace Zorro.Data.Interfaces;

public interface IDTO<TDTO>
{
    public TDTO MapToDTO(QueryContext context);
}