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

        public static string RegistrationServiceUri = "/reg/reg_phone?uri={0}";


        public static string LoginServiceUri = "profile/logon";
        public static string LogoutSericeUri = "profile/logout?username={0}";
        public static string GetUserInfoServiceUri = "";
        public static string GetMealPlanServiceUri = "";
        public static string GetSportPlanServiceUri = "";
        public static string UpdateMealStatusServiceUri = "";
        public static string UpdateMealPlanStatusServiceUri = "";
        public static string UpdateSportServiceUri = "";
        public static string UpdateSportPlanServiceUri = "";
        public static Uri ServerUri = new Uri("http://localhost:1962/");
        public static Uri GetServerPathUri(string path)
        {
            return new Uri(ServerUri, path);
        }
    }
}
