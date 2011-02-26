using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Widgets.Infrastructure;

namespace Gymnastika.Widgets
{
    public abstract class WidgetBase : IWidget
    {
        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if(_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        protected virtual void OnIsActiveChanged()
        {
            if(IsActiveChanged != null)
                IsActiveChanged(this, new EventArgs());
        }

        public event EventHandler IsActiveChanged;
        public abstract void Initialize();
    }
}
