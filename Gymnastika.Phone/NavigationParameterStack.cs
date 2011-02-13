using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace Gymnastika.Phone
{
    public static class NavigationParameterStack
    {
        static Stack<object> m_stack=new Stack<object>();
        static Stack<int> m_stackIndex = new Stack<int>();
        public static void ClearOne()
        {
            int lastIndex = m_stackIndex.Pop();
            while (m_stack.Count > lastIndex)
                m_stack.Pop();
        }
        public static object Pop()
        {
            if (m_stack.Count > m_stackIndex.Peek())
                return m_stack.Pop();
            else
                throw new IndexOutOfRangeException("I don't have any parameters for you.");
        }
        public static void Push(params object[] objects)
        {
            m_stackIndex.Push(m_stack.Count);
            for(int i=objects.Length-1;i>=0;i--)
                m_stack.Push(objects[i]);
        }
        public static object[] Pop(int Count)
        {
            object[] ret = new object[Count];
            while (Count-- > 0)
                ret[Count] = Pop();
            return ret;
        }
        public static K Pop<K>()
        {
            return (K)Pop();
        }
        public static K[] Pop<K>(int Count)
        {
            K[] ret = new K[Count];
            while (Count-- > 0)
                ret[Count] = (K)Pop();
            return ret;
        }
    }
}
