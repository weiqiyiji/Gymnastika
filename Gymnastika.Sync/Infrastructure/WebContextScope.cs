using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gymnastika.Data;

namespace Gymnastika.Sync.Infrastructure
{
    public class WebContextScope : IWorkContextScope
    {
        private const string CurrentKey = "CurrentWebContext";
        private IDictionary<string, object> _items;

        private WebContextScope()
        {
            _items = new Dictionary<string, object>();
        }

        public static WebContextScope Current
        {
            get 
            {
                WebContextScope scope = HttpContext.Current.Items[CurrentKey] as WebContextScope;
                if (scope == null)
                {
                    scope = new WebContextScope();
                    HttpContext.Current.Items[CurrentKey] = scope;
                }

                return scope;
            }
        }

        #region IWorkContextScope Members

        public event EventHandler Disposing;

        public IDictionary<string, object> Items
        {
            get { return _items; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (Disposing != null)
                Disposing(this, EventArgs.Empty);

            HttpContext.Current.Items[CurrentKey] = null;
            _items.Clear();
            _items = null;
        }

        #endregion
    }
}