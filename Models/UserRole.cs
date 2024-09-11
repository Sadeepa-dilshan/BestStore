namespace BestStore.Models
{
    public class UserRole
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CurrentRole { get; set; }
        public List<string> AvailableRoles { get; set; }
    }
}
