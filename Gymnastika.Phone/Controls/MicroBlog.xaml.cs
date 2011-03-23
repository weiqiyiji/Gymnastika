using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gymnastika.Phone.Controls
{
    public partial class MicroBlog : UserControl
    {
        
        private bool m_IsOpen=false;
        public bool IsOpen
        {
            get { return m_IsOpen; }
            protected set { m_IsOpen = value; }
        }
        public double MaxMarginBottom { get; set; }
        public MicroBlog()
        {
            InitializeComponent();
            rectDrag.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(rectDrag_ManipulationStarted);
            rectDrag.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(rectDrag_ManipulationDelta);
            rectDrag.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(rectDrag_ManipulationCompleted);
            IsOpen = false;
            MaxMarginBottom = Margin.Bottom;
          
        }
        Storyboard StoryBoardOpenAndClose = new Storyboard();
        public void Open(bool NoDelay)
        {
            if (!NoDelay)
                Open();
        }
        public void Open()
        {

        }

        void Open_Completed(object sender, EventArgs e)
        {
            IsOpen = true;   
        }
        public void Close(bool NoDelay)
        {
            if (!NoDelay)
                Close();
        }
        public void Close()
        {

        }

        void Close_Completed(object sender, EventArgs e)
        {
            IsOpen = false;
        }
        void rectDrag_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

            
        }

        void rectDrag_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            this.Height += e.DeltaManipulation.Translation.Y;
        }

        void rectDrag_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {

        }
    }
}
