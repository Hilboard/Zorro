namespace Zorro.Data.Interfaces;

public interface IUpdateable<TForm> where TForm : notnull
{
    public bool UpdateFill(TForm form);
}