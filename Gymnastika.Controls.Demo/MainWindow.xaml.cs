using System;
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
using System.Reflection;

namespace Gymnastika.Controls.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DemoNamespace = "Gymnastika.Controls.Demo.Sub.";
        private Assembly _controlAssembly;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (_controlAssembly == null)
                _controlAssembly = Assembly.GetExecutingAssembly();

            string windowName = button.Content.ToString() + "Demo";

            Window targetWindow = (Window)_controlAssembly.CreateInstance(DemoNamespace + windowName);
            targetWindow.Show();
        }
    }
}
