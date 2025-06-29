namespace Zorro.Data.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class IncludeWhenAttribute : Attribute
{
    public readonly string VARIABLE_NAME;
    public readonly bool EXPECTED_VALUE;
    public readonly bool MAY_EXPECT_NULL;

    public IncludeWhenAttribute(string VARIABLE_NAME, bool EXPECTED_VALUE = true, bool MAY_EXPECT_NULL = false)
    {
        this.VARIABLE_NAME = VARIABLE_NAME;
        this.EXPECTED_VALUE = EXPECTED_VALUE;
        this.MAY_EXPECT_NULL = MAY_EXPECT_NULL;
    }
}