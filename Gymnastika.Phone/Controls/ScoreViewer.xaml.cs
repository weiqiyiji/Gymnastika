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
namespace Gymnastika.Phone.Controls
{
    public class ScoreItemCollection : ICollection<ScoreItem>
    {
        private ScoreViewer m_Parent;
        private List<ScoreItem> m_Items = new List<ScoreItem>();
        public ScoreItemCollection(ScoreViewer Viewer)
        {
            this.m_Parent = Viewer;
        }
        public void Add(ScoreItem item)
        {
            if (!m_Parent.OnItemAdd(item))
                return;
            m_Items.Add(item);
        }

        public void Clear()
        {
            if (!m_Parent.OnItemClear())
                return;
            m_Items.Clear();
        }

        public bool Contains(ScoreItem item)
        {
            return m_Items.Contains(item);
        }

        public void CopyTo(ScoreItem[] array, int arrayIndex)
        {
            m_Items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ScoreItem item)
        {
            if (!m_Parent.OnItemRemove(item))
                return false;
            return m_Items.Remove(item);
        }
        public IEnumerator<ScoreItem> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }
        public ScoreItem this[int index]
        {
            get { return m_Items[index]; }
        }
    }

    public class ScoreItem
    {
        internal ScoreViewer Viewer { get; set; }
        public int Index { get; internal set; }
        public string Name { get; set; }
        public ScheduleItemStatus Status { get; set; }
    }
    public partial class ScoreViewer : UserControl
    {
        #region variables
        private ScoreItemCollection m_Items;
        public ScoreItemCollection Items
        {
            get { return m_Items; }
        }
        #endregion

        public ScoreViewer()
        {
            m_Items = new ScoreItemCollection(this);
            InitializeComponent();
        }

        #region Items change handlers
        internal bool OnItemAdd(ScoreItem item)
        {
            item.Viewer = this;
            return true;
        }
        internal bool OnItemRemove(ScoreItem item)
        {
            item.Viewer = null;
            return true;
        }
        internal bool OnItemClear()
        {
            foreach (ScoreItem item in Items)
                item.Viewer = null;
            return true;
        }
        #endregion

        #region UI stuff

        #endregion
    }
}
