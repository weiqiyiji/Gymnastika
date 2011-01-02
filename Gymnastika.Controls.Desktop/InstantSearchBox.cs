using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Messaging;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace Gymnastika.Controls.Desktop
{
    [TemplatePart(Name = InstantSearchBox.PART_CancelButton, Type = typeof(Button))]
    public class InstantSearchBox : ComboBox
    {
        public const string PART_Popup = "PART_Popup";
        public const string PART_CancelButton = "PART_CancelButton";
        public const string PART_EditableTextBox = "PART_EditableTextBox";

        static InstantSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(InstantSearchBox), new FrameworkPropertyMetadata(typeof(InstantSearchBox)));
        }

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

        public bool IsPopupOpen
        {
            get { return (bool)GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanSearch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register("IsPopupOpen", typeof(bool), typeof(InstantSearchBox), new UIPropertyMetadata(false));



        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(InstantSearchBox), new UIPropertyMetadata(new CornerRadius(0)));

        

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

            if (searchBox != null)
            {
                searchBox.TextChanged += new TextChangedEventHandler(PART_EditableTextBox_TextChanged);
            }

            if (cancelButton != null)
            {
                cancelButton.Click += PART_CancelButton_Click;
            }
        }

        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Length > 0)
            {
                OpenPopup(true);

                if (DoSearch != null)
                {
                    DoSearch.BeginInvoke(textBox.Text, OnSearchCompleted, null);
                }
            }
            else
            {
                OpenPopup(false);
            }
        }

        private void OpenPopup(bool isOpen)
        {
            Popup popup = this.GetTemplateChild(PART_Popup) as Popup;
            if (popup != null)
            {
                IsPopupOpen = isOpen;
                popup.IsOpen = isOpen;
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

        private void PART_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = GetTemplateChild(PART_EditableTextBox) as TextBox;
            if (tb != null)
            {
                tb.Text = string.Empty;
            }
        }
    }

    public delegate ObservableCollection<object> SearchHandler(string query);
}
