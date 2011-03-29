using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Helpers
{
    public class AsychronousLoadHelper
    {
        public static void AsychronousCall(Action handler)
        {
            AsyncCallback callback = (result) => { };
            handler.BeginInvoke(callback, null);
        }
    }
}
