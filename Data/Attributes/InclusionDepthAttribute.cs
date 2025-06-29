namespace Zorro.Data.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class InclusionDepthAttribute : Attribute
{
    public readonly int DEPTH;

    public InclusionDepthAttribute(int DEPTH)
    {
        this.DEPTH = DEPTH;
    }
}