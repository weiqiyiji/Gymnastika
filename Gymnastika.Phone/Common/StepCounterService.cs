using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AccelerometerWP7ClassLibrary;
using System.Threading;

namespace Gymnastika.Phone.Common
{
    public class StepCounterService
    {
        AccelerometerSimulator senator = new AccelerometerSimulator();

        public event EventHandler StepChanged = delegate { };

        public event EventHandler<AccelerometerSimulatorReadingEventArgs> SenatorReadingChanged = delegate { };

        double maxThredHold = 11;
        double minThredHold = 5;
        bool canCount = false;

        public void Start()
        {
            senator.Start();
        }

        public void Stop()
        {
            senator.Stop();
        }

        public StepCounterService()
        {
            senator.ReadingChanged += new AccelerometerSimulator.EventHandler<AccelerometerSimulatorReadingEventArgs>(senator_ReadingChanged);
        }

        void senator_ReadingChanged(object sender, AccelerometerSimulatorReadingEventArgs e)
        {
            double length = GetLenght(e.X, e.Y, e.Z);
            if (length > maxThredHold && canCount)
            {
                if (StepChanged != null)
                    StepChanged(this, EventArgs.Empty);
                canCount = false;
            }
            else if (length < minThredHold)
            {
                canCount = true;
            }
            if (SenatorReadingChanged != null)
                SenatorReadingChanged(this, e);
        }

        double GetLenght(double x, double y, double z)
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
    }
}
