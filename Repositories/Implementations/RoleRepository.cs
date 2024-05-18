using KnowledgeNest.Context;
using KnowledgeNest.Entities.Identity;
using KnowledgeNest.Repositories.Interfaces;

namespace KnowledgeNest.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        protected readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Role AddRole(Role role)
        {
            _context.Roles.Add(role);
            return role;
        }

        public Role GetRole(string roleName)
        {
            return _context.Roles.FirstOrDefault(x => x.Name == roleName);
        }

        public IReadOnlyCollection<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
