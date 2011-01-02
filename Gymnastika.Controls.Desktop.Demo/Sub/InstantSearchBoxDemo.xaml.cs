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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Gymnastika.Controls.Desktop.Demo.Sub
{
    /// <summary>
    /// Interaction logic for InstantSearchBoxDemo.xaml
    /// </summary>
    public partial class InstantSearchBoxDemo : Window
    {
        private List<Item> _items = new List<Item> 
        {
            new Item { Name = "aaaaa", Value = "aaaaa" },
            new Item { Name = "aaabb", Value = "aaabb" },
            new Item { Name = "abbba", Value = "abbba" },
            new Item { Name = "bassb", Value = "bassb" },
            new Item { Name = "dsdadf", Value = "dsdadf" },
            new Item { Name = "vasdfasf", Value = "vasdfasf" },
            new Item { Name = "vasdfsadf", Value = "vasdfsadf" },
            new Item { Name = "vasfdgasdf", Value = "vasfdgasdf" },
            new Item { Name = "vasdf", Value = "vasdf" },
            new Item { Name = "vasdfwefa", Value = "vasdfwefa" },
            new Item { Name = "asdfa", Value = "asdfa" },
            new Item { Name = "asdfas", Value = "asdfas" },
            new Item { Name = "gjsggfd", Value = "gjsggfd" },
            new Item { Name = "sssdfa", Value = "sssdfa" },
            new Item { Name = "dagfgd", Value = "dagfgd" },
            new Item { Name = "sdgasgtrdddqa", Value = "sdgasgtrdddqa" },
            new Item { Name = "dadfsdafas", Value = "dadfsdafas" },
            new Item { Name = "ggehteet", Value = "ggehteet" },
            new Item { Name = "fdadfagghg", Value = "fdadfagghg" },
            new Item { Name = "afafadgrwwd", Value = "afafadgrwwd" },
            new Item { Name = "dagasfsdfsa", Value = "dagasfsdfsa" },
            new Item { Name = "wdfwqgfdassd", Value = "wdfwqgfdassd" },
            new Item { Name = "dsafgqwed", Value = "dsafgqwed" },
            new Item { Name = "dgqwedd", Value = "dgqwedd" }
        };

        public InstantSearchBoxDemo()
        {
            InitializeComponent();
            _items.Sort();
        }

        private ObservableCollection<object> OnDoSearch(string query)
        {
            return new ObservableCollection<object>(
                _items.Where(item => item.Name.Contains(query)));
        }
    }

    public class Item : IComparable<Item>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
 	         return Name;
        }

        #region IComparable<Item> Members

        public int CompareTo(Item other)
        {
            return this.Name.CompareTo(other.Name);
        }

        #endregion
    }
}
