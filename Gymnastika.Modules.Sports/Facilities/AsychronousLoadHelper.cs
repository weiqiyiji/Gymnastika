using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Sports.Facilities
{
    public static class AsychronousLoadHelper
    {
        public static void AsychronousResolve<T>(Action<T> callback,Dispatcher callbackDispather)
        {
            Func<T> resolveHandler = () =>
            {
                IServiceLocator locator = ServiceLocator.Current;
                return locator.GetInstance<T>();
            };
            AsyncCallback asyCallback = (IAsyncResult result) =>
                {
                    T instance = resolveHandler.EndInvoke(result);
                    callbackDispather.BeginInvoke(callback,DispatcherPriority.Background,instance);
                };
            resolveHandler.BeginInvoke(asyCallback, null);
        }

        

        public static void AsychronousCall(Action handler)
        {
            AsyncCallback callback = (result) => { };
            handler.BeginInvoke(callback, null);
        }

    }
}
