using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;

namespace iread_school_ms.DataAccess.Interface
{
    public interface ISchoolRepository
    {
        public Task<School> GetById(int id);

        public void Insert(School audio);

        public void Delete(int id);

        public bool Exists(int id);

        public void Update(School schoolEntity, School oldSchool);

    }
}