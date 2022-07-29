using System.Collections.Generic;

namespace task4.Models
{
    public class UsersViewModel
    {
        public List<UserInfo> Users { get; set; }

        public string ActivityProcent { get; set; }

        public string BlockedProcent { get; set; }

        public string DeleteProcent { get; set; }
    }
}
