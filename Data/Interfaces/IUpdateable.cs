namespace Zorro.Data.Interfaces;

public interface IUpdateable<TForm> where TForm : struct
{
    public bool UpdateFill(TForm form);
}