using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Interface;
using iread_school_ms.Web.Util;
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

        public async Task<List<Class>> GetAll()
        {
            return await _context.Classes.ToListAsync();
        }

        public bool ExistsStudent(string memberId)
        {
            return _context.ClassMembers.Any(m => m.MemberId == memberId);
        }

        public async Task<Class> GetById(int id, bool includeMembers)
        {
            if (includeMembers)
                return await _context.Classes
                .Include(c => c.School)
                .Include(c => c.Members)
                .SingleOrDefaultAsync(a => a.ClassId == id);

            return await _context.Classes
                .Include(c => c.School)
                .SingleOrDefaultAsync(c => c.ClassId == id);


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
            return _context.Classes.Any(c => c.ClassId == id);
        }

        public void Update(Class classObj, Class oldClass)
        {
            _context.Entry(oldClass).State = EntityState.Deleted;
            _context.Classes.Attach(classObj);
            _context.Entry(classObj).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<List<Class>> GetBySchool(int schoolId)
        {
            return await _context.Classes.Where(c => c.SchoolId == schoolId).ToListAsync();
        }

        public void Archive(Class classObj, bool archived)
        {
            classObj.Archived = archived;
            _context.SaveChanges();
        }

        public void ArchiveBySchool(int schoolId)
        {
            List<Class> notArchivedYet = _context.Classes.Where(c => c.SchoolId == schoolId && !c.Archived).ToList();
            if (notArchivedYet == null)
                return;

            notArchivedYet.ForEach(c => c.Archived = true);
            _context.SaveChanges();

        }

        public async Task<List<Class>> GetArchived(bool archived)
        {
            return await _context.Classes.Where(c => c.Archived == archived).ToListAsync();
        }
    }
}