using System.Linq;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace iread_school_ms.DataAccess.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Class> GetById(int id)
        {
            return await _context.Classes.Include(c => c.School).SingleOrDefaultAsync(a => a.ClassId == id);
        }

        public void Insert(Class classObj)
        {
            _context.Classes.Add(classObj);
            _context.SaveChangesAsync();
        }

        public void Delete(Class classObj)
        {
            _context.Classes.Remove(classObj);
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.Classes.Any(a => a.ClassId == id);
        }

        public void Update(Class classObj, Class oldClass)
        {
            _context.Entry(oldClass).State = EntityState.Deleted;
            _context.Classes.Attach(classObj);
            _context.Entry(classObj).State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}