namespace KnowledgeNest.Entities.Identity
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
