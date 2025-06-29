namespace Zorro.Data.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class AlwaysIncludeAttribute : Attribute
{

}