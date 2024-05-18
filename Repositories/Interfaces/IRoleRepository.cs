using KnowledgeNest.Entities.Identity;

namespace KnowledgeNest.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Role AddRole (Role role);   
        Role GetRole (string roleName);
        IReadOnlyCollection<Role> GetRoles ();
    }
}
