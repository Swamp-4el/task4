namespace task4.Helpers
{
    public static class UserStatus
    {
        public static string Active => "Active";

        public static string Blocked => "Blocked";

        public static string Delete => "Delete";

        public static string GetStatus(bool isBlocked, bool isDelete)
        {
            if (!isBlocked && !isDelete)
                return Active;

            if (isBlocked)
                return Blocked;

            return Delete;
        }
    }
}
