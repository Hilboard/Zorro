namespace Zorro.Data.Interfaces;

public interface IDataTransferObject<TDTO>
{
    public TDTO MapToDTO(object? argsObject = null);
}