using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace iread_school_ms.DataAccess.Repository
{
    public class SchoolMemberRepository : ISchoolMemberRepository
    {
        private readonly AppDbContext _context;

        public SchoolMemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SchoolMember> GetById(int id)
        {
            return await _context.SchoolMembers
            .Include(s => s.School)
            .SingleOrDefaultAsync(a => a.SchoolMemberId == id);
        }


        public void Insert(SchoolMember schoolMember)
        {
            _context.SchoolMembers.Add(schoolMember);
            _context.SaveChanges();
        }

        public void Delete(SchoolMember schoolMember)
        {
            _context.SchoolMembers.Remove(schoolMember);
            _context.SaveChanges();
        }


        public async Task<List<SchoolMember>> GetManagers(int schoolId)
        {
            return await _context.SchoolMembers
            .Where(s => s.SchoolId == schoolId)
            .ToListAsync();
        }
    }
}