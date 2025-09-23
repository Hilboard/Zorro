namespace Zorro.Data.Interfaces;

public interface IUpdateable<TForm>
{
    public bool UpdateFill(TForm form);
}