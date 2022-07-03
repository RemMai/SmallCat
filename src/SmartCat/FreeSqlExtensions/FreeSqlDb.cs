using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace SmartCat.FreeSqlExtensions
{
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
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

    }
}
