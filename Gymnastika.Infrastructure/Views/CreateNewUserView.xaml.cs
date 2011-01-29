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
    /// Interaction logic for CreateNewUserView.xaml
    /// </summary>
    public partial class CreateNewUserView : UserControl, ICreateNewUserView
    {
        public CreateNewUserView(CreateNewUserViewModel model)
        {
            InitializeComponent();
            Model = model;
        }

        public CreateNewUserViewModel Model
        {
            get { return DataContext as CreateNewUserViewModel; }
            set { DataContext = value; }
        }
    }
}