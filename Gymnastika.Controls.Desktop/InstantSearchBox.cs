using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Messaging;

namespace Gymnastika.Controls.Desktop
{
    [TemplatePart(Name = InstantSearchBox.PART_CancelButton, Type = typeof(Button))]
    [TemplatePart(Name = InstantSearchBox.PART_SearchButton, Type = typeof(Button))]
    public class InstantSearchBox : ComboBox
    {
        public const string PART_CancelButton = "PART_CancelButton";
        public const string PART_SearchButton = "PART_SearchButton";
        public const string PART_EditableTextBox = "PART_EditableTextBox";

        public Brush SearchResultsBackground
        {
            get { return (Brush)GetValue(SearchResultsBackgroundProperty); }
            set { SetValue(SearchResultsBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchResultsBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchResultsBackgroundProperty =
            DependencyProperty.Register("SearchResultsBackground", typeof(Brush), typeof(InstantSearchBox), new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255))));


        public ImageSource SearchImageSource
        {
            get { return (ImageSource)GetValue(SearchImageSourceProperty); }
            set { SetValue(SearchImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchImageSourceProperty =
            DependencyProperty.Register("SearchImageSource", typeof(ImageSource), typeof(InstantSearchBox), new UIPropertyMetadata(null));



        public bool CanSearch
        {
            get { return (bool)GetValue(CanSearchProperty); }
            set { SetValue(CanSearchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanSearch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanSearchProperty =
            DependencyProperty.Register("CanSearch", typeof(bool), typeof(InstantSearchBox), new UIPropertyMetadata(false));
 

        public SearchHandler DoSearch
        {
            get { return (SearchHandler)GetValue(DoSearchProperty); }
            set { SetValue(DoSearchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DoSearch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoSearchProperty =
            DependencyProperty.Register("DoSearch", typeof(SearchHandler), typeof(InstantSearchBox), new UIPropertyMetadata(null)); 
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TextBox searchBox = GetTemplateChild(InstantSearchBox.PART_EditableTextBox) as TextBox;
            Button cancelButton = GetTemplateChild(InstantSearchBox.PART_CancelButton) as Button;
            Button searchButton = GetTemplateChild(InstantSearchBox.PART_SearchButton) as Button;

            if (searchBox != null)
            {
                searchBox.TextChanged += new TextChangedEventHandler(PART_EditableTextBox_TextChanged);
            }

            if (cancelButton != null)
            {
                cancelButton.Click += PART_CancelButton_Click;
            }

            if (searchButton != null)
            {
                searchButton.Click += PART_SearchButton_Click;
            }
        }

        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Length > 0)
            {
                if (DoSearch != null)
                {
                    DoSearch.BeginInvoke(textBox.Text, OnSearchCompleted, null);
                }
            }
        }

        private void OnSearchCompleted(IAsyncResult result)
        {
            SearchHandler doSearch = (SearchHandler)((AsyncResult)result).AsyncDelegate;
            ObservableCollection<object> searchResults = doSearch.EndInvoke(result);
            this.Dispatcher.BeginInvoke(
                (Action)(() => 
                {
                    this.ItemsSource = searchResults;
                }));
        }

        private void PART_SearchButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void PART_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public delegate ObservableCollection<object> SearchHandler(string query);
}
