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
    public partial class Pedometer : UserControl
    {
        Common.StepCounterService counterService = new Common.StepCounterService();

        private static double METRIC_RUNNING_FACTOR = 1.02784823;
        private static double IMPERIAL_RUNNING_FACTOR = 0.75031498;

        private static double METRIC_WALKING_FACTOR = 0.708;
        private static double IMPERIAL_WALKING_FACTOR = 0.517;
        private int m_StepCount;
        public int StepCount
        {
            get;
            private set;
        }
        public bool IsCounting { get; private set; }
        public double BodyWeight { get; set; }
        /// <summary>
        /// Step length in CM
        /// </summary>
        public double StepLength { get; set; }
        public bool IsRunning { get; set; }
        /// <summary>
        /// Distance in M
        /// </summary>
        public double Distance { get; private set; }
        public double Calory { get; private set; }
        /// <summary>
        /// speed   km/h
        /// </summary>
        public double Speed
        {
            get
            {
                return Pace * StepLength * 60 / 100000;
            }
        }
        /// <summary>
        /// Steps per min
        /// </summary>
        public double Pace
        {
            get
            {
                double steps = 0, time = 0;
                LinkedListNode<double> curNode = lastSteps.First;
                for (curNode = lastSteps.First; curNode != null; curNode = curNode.Next)
                {
                    ++steps;
                    time += curNode.Value;
                }
                if (time > 0)
                    return steps / time;
                else
                    return 0;
            }
        }
        private DateTime lastStepTime = DateTime.MinValue;
        LinkedList<double> lastSteps = new LinkedList<double>();
        private void AddSetp()
        {
            ++StepCount;
            if (StepCount > 1)
            {
                if (lastSteps.Count >= 50)
                    lastSteps.Remove(lastSteps.First);
                lastSteps.AddLast((DateTime.Now - lastStepTime).TotalMinutes);
            }
            lastStepTime = DateTime.Now;
            Distance += StepLength / 100;
            Calory += BodyWeight * (IsRunning ? METRIC_RUNNING_FACTOR : METRIC_WALKING_FACTOR)
                * StepLength / 100000 * 2.2;
            RefreshCounter();
        }
        public Pedometer()
        {
            InitializeComponent();
            counterService.StepChanged += new EventHandler(counterService_StepChanged);
            IsCounting = false;

        }
        private void RefreshCounter()
        {
            runCalory.Text = Calory.ToString("0.00");
            runDistance.Text = Distance.ToString("0.00");
            runSpeed.Text = Speed.ToString("0.00") + "km/h";
            txtStep.Text = StepCount.ToString();

        }
        void counterService_StepChanged(object sender, EventArgs e)
        {
            AddSetp();
        }
        public void Start()
        {
            counterService.Start();
            IsCounting = true;
        }
        public void Pause()
        {
            counterService.Stop();
            IsCounting = false;
        }
        public void Reset()
        {
            StepCount = 0;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            RefreshCounter();
        }

    }
}
