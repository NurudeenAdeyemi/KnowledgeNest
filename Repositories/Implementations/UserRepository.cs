using KnowledgeNest.Context;
using KnowledgeNest.Entities.Identity;
using KnowledgeNest.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeNest.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        protected readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public User AddUser(User user)
        {
            _context.Users.Add(user);
            return user;
        }

        public User FindByEmail(string email)
        {
            return _context.Users.Include(x => x.UserRoles).ThenInclude(r => r.Role).SingleOrDefault(x => x.Email.ToLower() == email.ToLower());
        }

        public User FindById(int id)
        {
            return _context.Users.Find(id);
        }

        public User FindByName(string name)
        {
            return _context.Users.Include(x => x.UserRoles).ThenInclude(r => r.Role).SingleOrDefault(x => x.UserName.ToLower() == name.ToLower());
        }
    }
}
