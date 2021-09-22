using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Data.Type;
using iread_school_ms.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace iread_school_ms.DataAccess.Repository
{
    public class ClassMemberRepository : IClassMemberRepository
    {
        private readonly AppDbContext _context;

        public ClassMemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClassMember> GetById(int id)
        {
            return await _context.ClassMembers
            .Include(c => c.Class)
            .SingleOrDefaultAsync(a => a.ClassMemberId == id);
        }


        public void Insert(ClassMember classMember)
        {
            _context.ClassMembers.Add(classMember);
            _context.SaveChanges();
        }

        public void Delete(ClassMember classMember)
        {
            _context.ClassMembers.Remove(classMember);
            _context.SaveChanges();
        }

        public void Update(ClassMember classMember)
        {
            _context.ClassMembers.Update(classMember);
            _context.SaveChanges();
        }

        public List<Class> GetByStudent(string studentId)
        {
            return _context.ClassMembers
            .Include(c => c.Class)
            .Where(cm => cm.MemberId == studentId
            && cm.ClassMembershipType == ClassMembershipType.Student.ToString())
            .Select(cm => new Class() { ClassId = cm.ClassId, Title = cm.Class.Title })
            .ToList();
        }

        public List<Class> GetByTeacher(string teacherId)
        {
            return _context.ClassMembers
            .Include(c => c.Class)
            .Where(cm => cm.MemberId == teacherId
            && cm.ClassMembershipType == ClassMembershipType.Teacher.ToString())
            .Select(cm => new Class() { ClassId = cm.ClassId, Title = cm.Class.Title })
            .ToList();
        }
    }
}