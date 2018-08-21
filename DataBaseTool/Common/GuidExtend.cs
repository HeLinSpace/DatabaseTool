using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTool.Common
{
    public static class GuidExtend
    {
        public static string NewId()
        {

            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
