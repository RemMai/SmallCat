namespace SmartCat.DynamicWebApi;
[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class NonDynamicWebApiAttribute : Attribute { }