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

namespace Gymnastika.Controls.Desktop.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DemoNamespace = "Gymnastika.Controls.Desktop.Demo.Sub.";
        private Assembly _controlAssembly;
        private IDictionary<string, Window> _demoWindowCache;

        public MainWindow()
        {
            InitializeComponent();
            _demoWindowCache = new Dictionary<string, Window>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (_controlAssembly == null)
                _controlAssembly = Assembly.GetExecutingAssembly();

            string windowName = button.Content.ToString() + "Demo";

            Window targetWindow = GetTargetWindow(DemoNamespace + windowName);
            targetWindow.Show();
        }

        private Window GetTargetWindow(string windowName)
        {
            Window targetWindow = null;

            if (!_demoWindowCache.TryGetValue(windowName, out targetWindow))
            {
                targetWindow = (Window)_controlAssembly.CreateInstance(windowName);
                _demoWindowCache.Add(windowName, targetWindow);
            }

            return targetWindow;
        }
    }
}
