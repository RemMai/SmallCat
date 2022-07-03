using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Model;

public class FreeSqlEntityGroup
{
    public Type Locker { get; internal set; }
    public List<Type> Entities { get; internal set; }
}
