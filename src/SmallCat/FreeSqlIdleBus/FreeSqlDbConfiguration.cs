using FreeSql;

namespace SmallCat.FreeSqlIdleBus;

/// <summary>
/// 数据库配置实体
/// </summary>
public class FreeSqlDbConfiguration
{
    /// <summary>
    /// 数据库锁
    /// </summary>
    public Type Locker { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public DataType FreeSqlDataType { get; set; }

    /// <summary>
    /// 自动迁移数据库
    /// </summary>
    public bool AutoSyncStructure { get; set; } = false;

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }

}
public class FreeSqlConnectionString
{
    /// <summary>
    /// Config
    /// </summary>
    public string Locker { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public int FreeSqlDataType { get; set; }

    /// <summary>
    /// 自动迁移数据库
    /// </summary>
    public bool AutoSyncStructure { get; set; } = false;

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }

}
