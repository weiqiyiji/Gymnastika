using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Surface.Presentation.Controls;

namespace Gymnastika.Surface.Demo
{
    public class TestDataCollection : ObservableCollection<BitmapImage>
    {

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var ContentSelector = (SurfaceListBox) sender;
            Grid content = ((SurfaceListBoxItem)ContentSelector.SelectedItem).Tag as Grid;

            ContentArea.Children.Clear();
            ContentArea.Children.Add(content);
        }
    }
}
