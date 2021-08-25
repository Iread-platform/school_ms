using System.Collections.Generic;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;

namespace iread_school_ms.DataAccess.Interface
{
    public interface ISchoolMemberRepository
    {
        public Task<SchoolMember> GetById(int id);

        public void Insert(SchoolMember schoolMember);

        public void Delete(SchoolMember schoolMember);

        public Task<List<SchoolMember>> GetManagers(int schoolId);
        public Task<List<SchoolMember>> GetStudents(int schoolId);
        public Task<List<SchoolMember>> GetTeachers(int schoolId);
    }
}