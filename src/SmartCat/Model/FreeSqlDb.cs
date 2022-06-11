using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Model;

public class FreeSqlDb
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public int DbType { get; set; }
    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }
}
