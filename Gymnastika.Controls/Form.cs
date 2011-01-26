using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Collections.Specialized;

namespace Gymnastika.Controls
{
    //TODO: Unfinished

    [ContentProperty("FormItems")]
    [TemplatePart(Name = PART_Header, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_FormContent, Type = typeof(Grid))]
    public class Form : Control
    {
        protected const string PART_Header = "PART_Header";
        protected const string PART_FormContent = "PART_FormContent";
        private Grid _partFormContent;

        public Form()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));

            FormItems = new ObservableCollection<FormItem>();
            FormItems.CollectionChanged += FormItems_CollectionChanged;
        }

        private void FormItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ConstructGrid();
            
            ContentControl partHeader = GetTemplateChild(PART_Header) as ContentControl;
            if (partHeader != null)
            {
                partHeader.Content = Header;

                if (HeaderTemplate != null)
                    partHeader.ContentTemplate = HeaderTemplate;
            }
        }

        private void ConstructGrid()
        {
            _partFormContent = GetTemplateChild(PART_FormContent) as Grid;
            if (_partFormContent != null)
            {
                _partFormContent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                _partFormContent.ColumnDefinitions.Add(new ColumnDefinition());

                for (int i = 0; i < FormItems.Count; i++)
                {
                    _partFormContent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    Label label = new Label();
                    label.Content = FormItems[i].Label;
                    if (LabelStyle != null) label.Style = LabelStyle;

                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, 0);

                    TextBox tb = new TextBox();
                    if (InputBoxStyle != null) tb.Style = InputBoxStyle;

                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, 1);

                    _partFormContent.Children.Add(label);
                    _partFormContent.Children.Add(tb);
                }
            }
        }

        #region Dependency Properties

        public ObservableCollection<FormItem> FormItems
        {
            get { return (ObservableCollection<FormItem>)GetValue(FormItemsProperty); }
            set { SetValue(FormItemsProperty, value); }
        }

        public static readonly DependencyProperty FormItemsProperty =
            DependencyProperty.Register(
                "FormItems", typeof(ObservableCollection<FormItem>), typeof(Form), 
                new UIPropertyMetadata(null));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(Form), new UIPropertyMetadata(null));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Form), new UIPropertyMetadata(null));

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register("LabelStyle", typeof(Style), typeof(Form), new UIPropertyMetadata(null));     

        public Style InputBoxStyle
        {
            get { return (Style)GetValue(InputBoxStyleProperty); }
            set { SetValue(InputBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty InputBoxStyleProperty =
            DependencyProperty.Register("InputBoxStyle", typeof(Style), typeof(Form), new UIPropertyMetadata(null));

        #endregion
    }
}
