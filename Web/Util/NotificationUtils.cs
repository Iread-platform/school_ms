
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iread_school_ms.DataAccess.Data.Entity;
using System.Text.RegularExpressions;
using System.Linq;

namespace iread_school_ms.Web.Util
{
    public static class NotificationUtil
    {
        public static string SchoolTopicTitle(School school)
        {
            return ProcessTopicName(new string(school.SchoolId.ToString() + school.Title));
        }

        public static string ClassTopicTitle(Class obj)
        {
            return ProcessTopicName(new string(obj.ClassId + obj.SchoolId + obj.Title));
        }


        public static string SchoolTeachersTopicTitle(School school)
        {
            return ProcessTopicName(new string(school.SchoolId + "TEACHERS"));
        }

        public static string ClassTeachersTopicTitle(Class obj)
        {
            return ProcessTopicName(new string(obj.ClassId + "TEACHERS"));
        }

        private static string ProcessTopicName(string name)
        {
            Regex rgx = new Regex(@"[a-zA-Z0-9-_.~%]+");
            var cahrs = name.Where((character) => rgx.IsMatch(character.ToString()));
            string processedName = new string(cahrs.ToArray());
            return processedName;
        }

    }
}