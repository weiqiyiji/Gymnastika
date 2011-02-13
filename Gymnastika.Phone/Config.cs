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

namespace Gymnastika.Phone
{
    public class Config
    {
        /// <summary>
        /// The folder used to sotre users' profile. Doesn't contains last '\'
        /// </summary>
        public static string StoredProfileFolder = "Profile";
        /// <summary>
        /// The path of a user's profile file. Use string.Format(me,Username)
        /// </summary>
        public static string StoredProfileFilename = StoredProfileFolder + "\\{0}\\Stored.db";
        /// <summary>
        /// The folder used to sotre users'  icon. Doesn't contains last '\'
        /// </summary>
        public static string StoredProfileIconFolder = StoredProfileIconFolder + "\\Icons";
        /// <summary>
        /// The path of a user's profile icon. Use string.Format(me,Username)
        /// </summary>
        public static string StoredProfileIconFilename = StoredProfileIconFolder + "\\{0}.jpg";

    }
}
