using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data
{
    public interface IWorkContextScope : IDisposable 
    {
        event EventHandler Disposing;
        IDictionary<string, object> Items { get; }
    }

    public class WorkContextScope : IWorkContextScope
    {
        [ThreadStatic]
        private static WorkContextScope _current;

        public IDictionary<string, object> Items { get; private set; }

        private WorkContextScope() 
        {
            Items = new Dictionary<string, object>();
        }

        public static WorkContextScope Current 
        {
            get 
            {
                if (_current == null)
                    _current = new WorkContextScope();

                return _current; 
            }
        }

        public event EventHandler Disposing;

        #region IDisposable Members

        public void Dispose()
        {
            if (Disposing != null)
                Disposing(this, new EventArgs());

            _current = null;

            Items.Clear();
            Items = null;
        }

        #endregion
    }
}
