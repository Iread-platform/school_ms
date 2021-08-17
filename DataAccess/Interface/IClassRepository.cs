using System.Collections.Generic;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;

namespace iread_school_ms.DataAccess.Interface
{
    public interface IClassRepository
    {
        public Task<Class> GetById(int id, bool includeMambers);

        public Task<List<Class>> GetBySchool(int schoolId);

        public void Insert(Class classObj);

        public void Delete(Class classObj);

        public void Archive(Class classObj);

        public bool Exists(int id);

        public void Update(Class classEntity, Class oldClass);
        public void ArchiveBySchool(int schoolId);
    }
}