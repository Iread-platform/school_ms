using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Extensions;
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

        public async Task<Class> GetById(int id, bool includeMambers)
        {
            if (includeMambers)
                return await _context.Classes
                .Include(c => c.School)
                .Include(c => c.Members)
                .SingleOrDefaultAsync(a => a.ClassId == id);

            return await _context.Classes
                .Include(c => c.School)
                .SingleOrDefaultAsync(c => c.ClassId == id && !c.Archived);


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
            return _context.Classes.Any(c => c.ClassId == id && !c.Archived);
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
            return await _context.Classes.Where(c => c.SchoolId == schoolId && !c.Archived).ToListAsync();
        }

        public void Archive(Class classObj)
        {
            classObj.Archived = true;
            _context.SaveChanges();
        }

        public void ArchiveBySchool(int schoolId)
        {
            _context.Classes.Where(c => c.SchoolId == schoolId && !c.Archived)
            .Update(c => new Class { Archived = true });
            _context.SaveChanges();
        }

    }
}