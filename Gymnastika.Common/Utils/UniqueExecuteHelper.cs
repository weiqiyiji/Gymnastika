using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.Utils
{
    public class UniqueExecuteHelper
    {
        public static void Execute(params Func<bool>[] actions)
        {
            foreach (Func<bool> action in actions)
            {
                if (action()) return;
            }
        }
    }
}
