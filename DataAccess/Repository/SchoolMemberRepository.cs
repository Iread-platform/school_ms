using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Data.Type;
using iread_school_ms.DataAccess.Interface;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.School;
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
            .Where(s => s.SchoolId == schoolId
            && s.SchoolMembershipType == SchoolMembershipType.SchoolManager.ToString())
            .ToListAsync();
        }

        public async Task<List<SchoolMember>> GetStudents(int schoolId)
        {
            return await _context.SchoolMembers
            .Where(s => s.SchoolId == schoolId
            && s.SchoolMembershipType == SchoolMembershipType.Student.ToString())
            .ToListAsync();
        }

        public async Task<List<SchoolMember>> GetTeachers(int schoolId)
        {
            return await _context.SchoolMembers
            .Where(s => s.SchoolId == schoolId
            && s.SchoolMembershipType == SchoolMembershipType.Teacher.ToString())
            .ToListAsync();
        }

        public async Task<SchoolMember> GetByMemberId(string memberId)
        {
            return await _context.SchoolMembers
                .Where(s => s.MemberId == memberId)
                .Include(s => s.School)
                .FirstOrDefaultAsync();
        }
        public SchoolAndClassDto GetSchoolAndClassId(string memberId)
        {
            var student = from st in (from sm in _context.SchoolMembers
                    join s in _context.Schools on sm.SchoolId equals s.SchoolId //inner join with school
                    join cm in _context.ClassMembers on sm.MemberId equals cm.MemberId into classMembers //left join with class member
                    from subpet in classMembers.DefaultIfEmpty() //left join result
                    join c in _context.Classes on subpet.ClassId equals c.ClassId into classes//left join with class
                    from subpetClasses in classes.DefaultIfEmpty() //left join result
                    where sm.MemberId == memberId
                    select new
                    {
                        SchoolId = s.SchoolId,
                        ClassId = subpetClasses != null? subpetClasses.ClassId : -1,
                        ClassTitle = subpetClasses != null? subpetClasses.Title : String.Empty,
                        SchoolTitle = s.Title,
                        Archived = subpetClasses != null && subpetClasses.Archived,
                        SchoolMembershipType = sm.SchoolMembershipType
                    }).AsEnumerable()
                   group new InnerClassDto
                    {
                       Title = st.ClassTitle,
                       ClassId = st.ClassId,
                       Archived = st.Archived
                    } by new{st.SchoolId,st.SchoolTitle, st.SchoolMembershipType}  into g
                    select new SchoolAndClassDto
                    {
                        SchoolId = g.Key.SchoolId,
                        SchoolTitle = g.Key.SchoolTitle,
                        SchoolMembershipType = g.Key.SchoolMembershipType,
                        Classes = g.ToList()
                    };
            
            return student.First();
            
        }
        
        public void Update(SchoolMember schoolMemberEntity, SchoolMember oldSchoolMember)
        {
            // _context.Entry(oldSchoolMember).State = EntityState.Deleted;
            // _context.SchoolMembers.Attach(schoolMemberEntity);
            // _context.Entry(schoolMemberEntity).State = EntityState.Modified;
            oldSchoolMember.FirstName = schoolMemberEntity.FirstName;
            oldSchoolMember.LastName = schoolMemberEntity.LastName;
            _context.SaveChanges();
        }
    }
}