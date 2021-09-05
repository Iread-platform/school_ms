using System.Collections.Generic;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;

namespace iread_school_ms.DataAccess.Interface
{
    public interface IClassMemberRepository
    {
        public Task<ClassMember> GetById(int id);
        public List<Class> GetByStudent(string studentId);
        public List<Class> GetByTeacher(string teacherId);
        public void Insert(ClassMember classMember);
        public void Delete(ClassMember classMember);

    }
}