namespace Zorro.Data.Interfaces;

public interface IAddable<TForm> where TForm : notnull
{
    public bool AddFill(TForm form);
}