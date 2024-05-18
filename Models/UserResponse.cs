namespace KnowledgeNest.Models
{
    public class UserResponse : BaseResponse
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; }
    }
}
