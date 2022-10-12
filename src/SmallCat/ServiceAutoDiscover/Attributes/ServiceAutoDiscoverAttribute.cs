namespace SmallCat.ServiceAutoDiscover.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceAutoDiscoverAttribute : Attribute
{
    /// <summary>
    /// 生命周期
    /// </summary>
    public int Life { get; }

    /// <summary>
    /// 实现接口
    /// </summary>
    public Type ImplementationInterface { get; }

    /// <summary>
    /// ServiceAutoDiscover , Custom definition 
    /// </summary>
    /// <param name="life"> <para>1: Scoped </para> <para>2: Singleton</para><para>3: Transient</para> </param>
    /// <param name="implementationInterface"></param>
    public ServiceAutoDiscoverAttribute(int life, Type implementationInterface)
    {
        if (life < 1 || life > 3)
        {
            throw new Exception("ServiceAutoDiscover Value Error! Please Input 1 or 2 or 3.");
        }

        Life = life;
        ImplementationInterface = implementationInterface;
    }

    /// <summary>
    /// ServiceAutoDiscover, default Scoped
    /// </summary>
    /// <param name="implementationInterface"></param>
    public ServiceAutoDiscoverAttribute(Type implementationInterface)
    {
        Life = 1;
        ImplementationInterface = implementationInterface;
    }
}