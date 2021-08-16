using System.Collections.Generic;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;

namespace iread_school_ms.DataAccess.Interface
{
    public interface IClassMemberRepository
    {
        public Task<ClassMember> GetById(int id);

        public void Insert(ClassMember classObj);

        public void Delete(ClassMember classObj);

    }
}