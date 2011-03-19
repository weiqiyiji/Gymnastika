﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gymnastika.ViewModels;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for SyncView.xaml
    /// </summary>
    public partial class SyncView : UserControl
    {
        public SyncView(SyncViewModel model)
        {
            InitializeComponent();
            Model = model;
        }

        public SyncViewModel Model
        {
            get { return DataContext as SyncViewModel; }
            set { DataContext = value; }
        }
    }
}