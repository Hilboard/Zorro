namespace Zorro.Data.Interfaces;

public interface IAddable<TForm> where TForm : struct
{
    public bool AddFill(TForm form);
}