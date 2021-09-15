using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iread_school_ms.Web.Util
{
    public class Policies
    {

        public const string Administrator = "Administrator";
        public const string Teacher = "Teacher";
        public const string Student = "Student";
        public const string SchoolManager = "SchoolManager";

        public static AuthorizationPolicy AdministratorPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Administrator).Build();
        }
        public static AuthorizationPolicy TeacherPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Teacher).Build();
        }
        public static AuthorizationPolicy StudentPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Student).Build();
        }
        public static AuthorizationPolicy SchoolManagerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(SchoolManager).Build();
        }
    }
}
