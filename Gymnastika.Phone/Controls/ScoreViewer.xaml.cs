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
using Gymnastika.Phone.Common;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace Gymnastika.Phone.Controls
{


    public partial class ScoreViewer : UserControl
    {
        public ObservableCollection<ScheduleItem> ScoreItems { get; private set; }
        public ScoreViewer()
        {
            InitializeComponent();
            ScoreItems = new ObservableCollection<ScheduleItem>();
            FinishChange();
        }


        public static DependencyProperty ScoreProperty = DependencyProperty.Register("ScoreProperty", typeof(double), typeof(ScoreViewer),
            new PropertyMetadata(new PropertyChangedCallback((obj, e) =>
            {
                ScoreViewer viewer = obj as ScoreViewer;
                viewer.UpdateScore();
            }
            )));
        public static DependencyProperty CalorieInProperty = DependencyProperty.Register("CalorieInProperty", typeof(double), typeof(ScoreViewer),
            new PropertyMetadata(new PropertyChangedCallback((obj, e) =>
            {
                (obj as ScoreViewer).UpdateCalorieIn();
            })));
        public static DependencyProperty CalorieOutProperty = DependencyProperty.Register("CalorieOutProperty", typeof(double), typeof(ScoreViewer),
            new PropertyMetadata(new PropertyChangedCallback((obj, e) =>
            {
                (obj as ScoreViewer).UpdateCalorieOut();
            })));
        public static DependencyProperty ProgressProperty = DependencyProperty.Register("ProgressProperty", typeof(double), typeof(ScoreViewer),
            new PropertyMetadata(new PropertyChangedCallback((obj, e) =>
            {
                (obj as ScoreViewer).UpdateProgress();
            })));

        public double Score
        {
            get { return (double)this.GetValue(ScoreProperty); }
            private set { this.SetValue(ScoreProperty, value); }
        }
        private double CalorieIn
        {
            get { return (double)this.GetValue(CalorieInProperty); }
        }
        private double CalorieOut
        {
            get { return (double)this.GetValue(CalorieOutProperty); }
        }
        private double Progress
        {
            get { return (double)this.GetValue(ProgressProperty); }
        }
        private void UpdateCalorieIn()
        {
            txtInCalorie.Text = Math.Round(CalorieIn, 1) + " 大卡";
        }
        private void UpdateCalorieOut()
        {
            txtOutCalorie.Text = Math.Round(CalorieOut, 1) + " 大卡";
        }
        private void UpdateProgress()
        {
             txtProgress.Text = Math.Round(Progress * 100, 0) + "%";
        }
        private void UpdateScore()
        {
            txtScore.Text = Math.Round((decimal)Score, 0).ToString();
        }
        Storyboard WidthStoryborad = new Storyboard();
        Storyboard DigitStoryboard = new Storyboard();

        DoubleAnimation cOutAnimation = new DoubleAnimation(),
            cInAnimation = new DoubleAnimation(),
            scoreAnimation = new DoubleAnimation(),
            calorieInAnimation = new DoubleAnimation(),
            calorieOutAnimation = new DoubleAnimation(),
            progressAnimation = new DoubleAnimation(),
            progressBarAnimation=new DoubleAnimation();
        void AnimateToStatus(double cOut, double cIn, double score, double Progress)
        {
            double scoreFrom = this.Score;
            double outTo, inTo;
            double outFrom, inFrom;
            double total = cOut + cIn;

            if (cOut < 0 || cIn < 0)
                throw new ArgumentException();
            if (cOut == 0)
                outTo = 0;
            else
                outTo = Math.Round(cOut / total * outBorderOuter.ActualWidth, 0);
            if (cIn == 0)
                inTo = 0;
            else
                inTo = Math.Round(cIn / total * inBorderOuter.ActualWidth, 0);

            outFrom = outBorder.ActualWidth;
            inFrom = inBorder.ActualWidth;

            progressBarAnimation.From = compeletionBorder.ActualWidth;
            progressBarAnimation.To = competionBorderOuter.ActualWidth * Progress;

            WidthStoryborad.Stop();
            WidthStoryborad.Children.Clear();
            scoreAnimation.From = scoreFrom;
            scoreAnimation.To = score;
            scoreAnimation.Duration = TimeSpan.FromSeconds(1);

            calorieInAnimation.From = this.CalorieIn;
            calorieInAnimation.To = cIn;
            calorieInAnimation.Duration = TimeSpan.FromSeconds(1);


            calorieOutAnimation.From = this.CalorieOut;
            calorieOutAnimation.To = cOut;
            calorieOutAnimation.Duration = TimeSpan.FromSeconds(1);


            progressAnimation.From = this.Progress;
            progressAnimation.To = Progress;
            progressAnimation.Duration = TimeSpan.FromSeconds(1);

            cOutAnimation.From = outFrom;
            cOutAnimation.To = outTo;
            cOutAnimation.Duration = TimeSpan.FromSeconds(1);

            cInAnimation.From = inFrom;
            cInAnimation.To = inTo;
            cInAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTarget(cOutAnimation, outBorder);
            Storyboard.SetTarget(cInAnimation, inBorder);
            Storyboard.SetTarget(progressBarAnimation, compeletionBorder);

            Storyboard.SetTargetProperty(cOutAnimation, new PropertyPath(Border.WidthProperty));
            Storyboard.SetTargetProperty(cInAnimation, new PropertyPath(Border.WidthProperty));
            Storyboard.SetTargetProperty(progressBarAnimation, new PropertyPath(Border.WidthProperty));
            //Storyboard.SetTarget(scoreAnimation, this);
           

            Storyboard.SetTargetProperty(scoreAnimation, new PropertyPath(ScoreProperty));
            Storyboard.SetTargetProperty(calorieOutAnimation, new PropertyPath(CalorieOutProperty));
            Storyboard.SetTargetProperty(calorieInAnimation, new PropertyPath(CalorieInProperty));
            Storyboard.SetTargetProperty(progressAnimation, new PropertyPath(ProgressProperty));

            WidthStoryborad.Children.Add(cInAnimation);
            WidthStoryborad.Children.Add(cOutAnimation);
            WidthStoryborad.Children.Add(progressBarAnimation);
            DigitStoryboard.Stop();
            DigitStoryboard.Children.Clear();
            DigitStoryboard.Children.Add(scoreAnimation);
            DigitStoryboard.Children.Add(calorieInAnimation);
            DigitStoryboard.Children.Add(calorieOutAnimation);
            DigitStoryboard.Children.Add(progressAnimation);
            Storyboard.SetTarget(DigitStoryboard, this);
            WidthStoryborad.Begin();
            DigitStoryboard.Begin();

        }
        public void FinishChange()
        {
            double cOut, cIn, score;
            cOut = cIn = score = 0;
            int compeletedCount = 0;
            if (ScoreItems.Count > 0)
            {
                foreach (ScheduleItem item in ScoreItems)
                {
                    if (item.Status == ScheduleItemStatus.Done)
                    {
                        if (item.Calorie < 0)
                            cOut -= item.Calorie;
                        else
                            cIn += item.Calorie;
                        score += item.Point;
                        compeletedCount++;
                    }
                }
                AnimateToStatus(cOut, cIn, score, (double)compeletedCount / ScoreItems.Count);
            }
            else
                AnimateToStatus(0, 0, 0, 0);
        }


    }
}
