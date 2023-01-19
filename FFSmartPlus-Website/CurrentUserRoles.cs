using System.Collections.Generic;

namespace FFSmartPlus_Website
{
    public class CurrentUserRoles
{
        public static List<string>? _role;

        public List<string>? Role { get { return _role; } set { _role = value; } }

    }
}
