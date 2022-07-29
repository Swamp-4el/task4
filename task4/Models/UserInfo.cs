using System;

namespace task4.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public bool IsSelected { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastLogin { get; set; }

        public string Status { get; set; }
    }
}
