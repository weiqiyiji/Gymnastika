using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation.Peers;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Media;

namespace Gymnastika.Phone.Controls
{
    public class ChildWindowAutomationPeer : FrameworkElementAutomationPeer, IWindowProvider, ITransformProvider
    {
        // Fields
        private bool _isTopMost;

        // Methods
        public ChildWindowAutomationPeer(ChildWindow owner)
            : base((FrameworkElement)owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this.RefreshIsTopMostProperty();
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Window;
        }

        protected override string GetClassNameCore()
        {
            return base.Owner.GetType().Name;
        }

        private bool GetIsTopMostCore()
        {
            return (this.OwningChildWindow.InteractionState != WindowInteractionState.BlockedByModalWindow);
        }

        protected override string GetNameCore()
        {
            string nameCore = base.GetNameCore();
            if (string.IsNullOrEmpty(nameCore))
            {
                AutomationPeer labeledByCore = this.GetLabeledByCore();
                if (labeledByCore != null)
                {
                    nameCore = labeledByCore.GetName();
                }
                if (string.IsNullOrEmpty(nameCore) && (this.OwningChildWindow.Title != null))
                {
                    nameCore = this.OwningChildWindow.Title.ToString();
                }
            }
            return nameCore;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if ((patternInterface == PatternInterface.Transform) || (patternInterface == PatternInterface.Window))
            {
                return this;
            }
            return base.GetPattern(patternInterface);
        }

        internal void RaiseInteractionStatePropertyChangedEvent(WindowInteractionState oldValue, WindowInteractionState newValue)
        {
            base.RaisePropertyChangedEvent(WindowPatternIdentifiers.WindowInteractionStateProperty, oldValue, newValue);
            this.RefreshIsTopMostProperty();
        }

        private void RefreshIsTopMostProperty()
        {
            this.IsTopMostPrivate = this.GetIsTopMostCore();
        }

        void ITransformProvider.Move(double x, double y)
        {
            if (x < 0.0)
            {
                x = 0.0;
            }
            if (y < 0.0)
            {
                y = 0.0;
            }
            if (x > this.OwningChildWindow.Width)
            {
                x = this.OwningChildWindow.Width;
            }
            if (y > this.OwningChildWindow.Height)
            {
                y = this.OwningChildWindow.Height;
            }
            FrameworkElement contentRoot = this.OwningChildWindow.ContentRoot;
            if (contentRoot != null)
            {
                GeneralTransform transform = contentRoot.TransformToVisual(null);
                if (transform != null)
                {
                    Point point = transform.Transform(new Point(0.0, 0.0));
                    TransformGroup renderTransform = contentRoot.RenderTransform as TransformGroup;
                    if (renderTransform == null)
                    {
                        renderTransform = new TransformGroup();
                        renderTransform.Children.Add(contentRoot.RenderTransform);
                    }
                    TranslateTransform transform2 = new TranslateTransform();
                    transform2.X = x - point.X;
                    transform2.Y = y - point.Y;
                    if (renderTransform != null)
                    {
                        renderTransform.Children.Add(transform2);
                        contentRoot.RenderTransform = renderTransform;
                    }
                }
            }
        }

        void ITransformProvider.Resize(double width, double height)
        {
        }

        void ITransformProvider.Rotate(double degrees)
        {
        }

        void IWindowProvider.Close()
        {
            this.OwningChildWindow.Close();
        }

        void IWindowProvider.SetVisualState(WindowVisualState state)
        {
        }

        bool IWindowProvider.WaitForInputIdle(int milliseconds)
        {
            return false;
        }

        // Properties
        private bool IsTopMostPrivate
        {
            get
            {
                return this._isTopMost;
            }
            set
            {
                if (this._isTopMost != value)
                {
                    this._isTopMost = value;
                    base.RaisePropertyChangedEvent(WindowPatternIdentifiers.IsTopmostProperty, !this._isTopMost, this._isTopMost);
                }
            }
        }

        private ChildWindow OwningChildWindow
        {
            get
            {
                return (ChildWindow)base.Owner;
            }
        }

        bool ITransformProvider.CanMove
        {
            get
            {
                return true;
            }
        }

        bool ITransformProvider.CanResize
        {
            get
            {
                return false;
            }
        }

        bool ITransformProvider.CanRotate
        {
            get
            {
                return false;
            }
        }

        WindowInteractionState IWindowProvider.InteractionState
        {
            get
            {
                return this.OwningChildWindow.InteractionState;
            }
        }

        bool IWindowProvider.IsModal
        {
            get
            {
                return true;
            }
        }

        bool IWindowProvider.IsTopmost
        {
            get
            {
                return this.IsTopMostPrivate;
            }
        }

        bool IWindowProvider.Maximizable
        {
            get
            {
                return false;
            }
        }

        bool IWindowProvider.Minimizable
        {
            get
            {
                return false;
            }
        }

        WindowVisualState IWindowProvider.VisualState
        {
            get
            {
                return WindowVisualState.Normal;
            }
        }
    }
}
