using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.FreeSql;

public interface IDataEntity { }
public interface IDataEntity<T> where T : IDbLocker { }
