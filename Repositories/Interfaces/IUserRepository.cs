using KnowledgeNest.Entities.Identity;

namespace KnowledgeNest.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User AddUser(User user);
        User FindById(int id);
        User FindByName(string name);
        User FindByEmail(string email);
    }
}
