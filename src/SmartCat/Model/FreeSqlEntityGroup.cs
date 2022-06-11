using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Model;

public class FreeSqlEntityGroup
{
    public string Key { get; internal set; }
    public List<Type> Types { get; internal set; }
}
