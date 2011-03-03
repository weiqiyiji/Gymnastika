using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public class WidgetDescriptor
    {
        public WidgetDescriptor(Type widgetType)
        {
            WidgetType = widgetType;
            Initialize();
        }

        private void Initialize()
        {
            WidgetMetadataAttribute widgetMetadata =
                (WidgetMetadataAttribute)WidgetType.GetCustomAttributes(typeof(WidgetMetadataAttribute), true).SingleOrDefault();
        
            if(widgetMetadata == null)
            {
                throw new InvalidOperationException(
                    string.Format("{0} initialize faild: missing WidgetMetadataAttribute"));
            }

            DisplayName = widgetMetadata.DisplayName;
            Icon = widgetMetadata.Icon;
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaiseIsActiveChanged();
                }
            }
        }

        public event EventHandler IsActiveChanged;

        public Type WidgetType { get; private set; }

        public string DisplayName { get; set; }

        public string Icon { get; set; }

        public int ZIndex { get; set; }

        public Point Position { get; set; }

        protected void RaiseIsActiveChanged()
        {
            if (IsActiveChanged != null)
                IsActiveChanged(this, EventArgs.Empty);
        }
    }
}
