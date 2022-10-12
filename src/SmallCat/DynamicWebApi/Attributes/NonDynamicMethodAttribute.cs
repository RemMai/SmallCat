namespace SmallCat.DynamicWebApi;

[Serializable, AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class NonDynamicMethodAttribute : Attribute { }
