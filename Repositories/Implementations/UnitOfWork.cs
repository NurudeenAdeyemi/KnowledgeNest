using KnowledgeNest.Context;
using KnowledgeNest.Repositories.Interfaces;

namespace KnowledgeNest.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
