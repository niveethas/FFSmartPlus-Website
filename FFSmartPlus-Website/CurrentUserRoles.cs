using System.Collections.Generic;

namespace FFSmartPlus_Website
{
    public class CurrentUserRoles
{
        public static List<string>? _role;
        public static string? _user;
        public List<string>? Role { get { return _role; } set { _role = value; } }
        public string? User { get { return _user; }set { _user = value; } }
    }
}
