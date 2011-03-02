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
using Gymnastika.Phone.Views;
using System.Reflection;
using Microsoft.Phone.Controls;
namespace Gymnastika.Phone
{
    public class Pages
    {
        static Dictionary<Type, Uri> m_pages = new Dictionary<Type, Uri>();
        static Dictionary<string, Type> m_types = new Dictionary<string, Type>();
        static Pages()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.Namespace == "Gymnastika.Phone.Views")
                {
                    if ((t.Attributes & TypeAttributes.Public) == TypeAttributes.Public)
                    {
                        if (t.BaseType == typeof(PhoneApplicationPage))
                        {

                            m_types.Add(t.Name.ToLower(), t);
                            m_pages.Add(t, new Uri(
                                string.Format("/views/{0}.xaml", t.Name), UriKind.Relative));
                        }
                    }
                }
            }
            /*
            m_types.Add("loginpage", typeof(LoginPage));
            m_pages.Add(typeof(LoginPage), new Uri("/views/LoginPage.xaml", UriKind.RelativeOrAbsolute));
            m_types.Add("loginprogesspage", typeof(LoginProgressPage));
            m_pages.Add(typeof(LoginProgressPage), new Uri("/views/LoginProgressPage.xaml", UriKind.RelativeOrAbsolute));
            m_types.Add("mainpage", typeof(MainPage));
            m_pages.Add(typeof(MainPage), new Uri("/views/MainPage.xaml", UriKind.RelativeOrAbsolute));
             */
        }
        public static Uri GetPageUri<K>()
        {
            return GetPageUri(typeof(K));
        }
        public static Uri GetPageUri(string PageName)
        {
            return m_pages[m_types[PageName.ToLower()]];
        }
        public static Uri GetPageUri(Type PageType)
        {
            return m_pages[PageType];
        }
        public static Uri GetPageUri<K>(string[] ParamNames,params string[] ParamValues)
        {
            return GetPageUri(typeof(K), ParamNames, ParamValues);
        }

        public static Uri GetPageUri(Type PageType, string[] ParamNames,params string[] ParamValues)
        {
            if (ParamNames.Length != ParamValues.Length)
                throw new ArgumentOutOfRangeException("ParamValues");
            if (ParamNames == null)
                throw new ArgumentNullException("ParamNames");
            if (ParamValues == null)
                throw new ArgumentNullException("ParamValues");
            string query = "?";
            for (int i = 0; i < ParamNames.Length; i++)
            {
                query += ParamNames[i] + "=" +  HttpUtility.UrlEncode(ParamValues[i]) + "&";
            }
           
            query=query.TrimEnd(new char[] { '&' });
            return new Uri(GetPageUri(PageType).OriginalString + query,UriKind.RelativeOrAbsolute);
        }

    }
}
